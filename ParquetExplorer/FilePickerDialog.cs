using ParquetExplorer.Models;
using ParquetExplorer.Services.Interfaces;

namespace ParquetExplorer
{
    /// <summary>
    /// Unified modal dialog that lets the user pick a Parquet or text file from
    /// three different sources — local filesystem, SFTP server, or Azure Blob Storage
    /// — without opening separate dialogs.
    /// <para>
    /// After <see cref="Form.ShowDialog"/> returns <see cref="DialogResult.OK"/>, the
    /// caller should read <see cref="SelectedFilePath"/>, <see cref="SelectedDisplayName"/>,
    /// and <see cref="IsTempFile"/>.  When <see cref="IsTempFile"/> is <c>true</c> the
    /// caller is responsible for deleting the file when it is done with it.
    /// </para>
    /// </summary>
    public partial class FilePickerDialog : Form
    {
        private readonly IAzureAccountService _azureAccountService;
        private readonly IAzureBlobService    _azureBlobService;
        private readonly IAzureSessionManager _sessionManager;
        private readonly ISftpService         _sftpService;

        // ── Result ─────────────────────────────────────────────────────────
        /// <summary>Local path of the selected file (may be a downloaded temp copy).</summary>
        public string? SelectedFilePath    { get; private set; }
        /// <summary>Human-readable source description to show in labels / titles.</summary>
        public string? SelectedDisplayName { get; private set; }
        /// <summary>True when the file at <see cref="SelectedFilePath"/> is a temp file that should be deleted by the caller.</summary>
        public bool    IsTempFile          { get; private set; }

        // ── SFTP state ─────────────────────────────────────────────────────
        private string _sftpCurrentPath = "/";
        private IReadOnlyList<SftpFileEntry> _sftpEntries = Array.Empty<SftpFileEntry>();
        private string? _sftpSelectedFile; // full path of the selected file entry

        // ── Azure state ────────────────────────────────────────────────────
        private IReadOnlyList<StorageAccountInfo> _azureAccounts = Array.Empty<StorageAccountInfo>();
        /// <summary>Virtual-folder prefix being browsed in the blob list (empty = container root).</summary>
        private string _azureCurrentPrefix = string.Empty;

        // ── Helpers ────────────────────────────────────────────────────────
        private sealed class BlobItem
        {
            public bool   IsBack    { get; init; }
            public bool   IsFolder  { get; init; }
            public string FullPath  { get; init; } = "";
            public string Display   { get; init; } = "";
            public override string ToString() =>
                IsBack ? "📁  .." : IsFolder ? $"📁  {Display}" : $"📄  {Display}";
        }

        private static readonly string FileFilter =
            "Parquet & Text Files (*.parquet;*.csv;*.tsv;*.txt)|*.parquet;*.csv;*.tsv;*.txt" +
            "|Parquet Files (*.parquet)|*.parquet" +
            "|CSV Files (*.csv)|*.csv" +
            "|TSV Files (*.tsv)|*.tsv" +
            "|Text Files (*.txt)|*.txt" +
            "|All Files (*.*)|*.*";

        public FilePickerDialog(
            IAzureAccountService azureAccountService,
            IAzureBlobService    azureBlobService,
            IAzureSessionManager sessionManager,
            ISftpService         sftpService)
        {
            _azureAccountService = azureAccountService;
            _azureBlobService    = azureBlobService;
            _sessionManager      = sessionManager;
            _sftpService         = sftpService;
            InitializeComponent();
        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (_azureAccountService.IsSignedIn)
            {
                btnAzureSignIn.Text = "🔄 Sign in again";
                lblAzureStatus.Text = "Already signed in — loading storage accounts...";
                await LoadAzureAccountsAsync();
            }
        }

        // ── Tab change ─────────────────────────────────────────────────────

        private void tabSources_SelectedIndexChanged(object? sender, EventArgs e)
        {
            // Re-evaluate whether the Select button should be enabled.
            UpdateSelectButton();
        }

        // ═══════════════════════════════════════════════════════════════════
        // LOCAL TAB
        // ═══════════════════════════════════════════════════════════════════

        private void btnBrowseLocal_Click(object sender, EventArgs e)
        {
            using var dlg = new OpenFileDialog
            {
                Title  = "Select File",
                Filter = FileFilter,
            };

            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            txtLocalPath.Text = dlg.FileName;
            UpdateSelectButton();
        }

        // ═══════════════════════════════════════════════════════════════════
        // SFTP TAB
        // ═══════════════════════════════════════════════════════════════════

        private async void btnSftpConnect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSftpHost.Text))
            {
                MessageBox.Show("Please enter a hostname.", "Missing Input",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtSftpPort.Text.Trim(), out int port) || port <= 0 || port > 65535)
            {
                MessageBox.Show("Please enter a valid port number (1 – 65535).", "Invalid Port",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _sftpCurrentPath = "/";
            txtSftpPath.Text = _sftpCurrentPath;
            await LoadSftpDirectoryAsync();
        }

        private async void btnSftpUp_Click(object sender, EventArgs e)
        {
            _sftpCurrentPath = GetParentPath(_sftpCurrentPath);
            txtSftpPath.Text = _sftpCurrentPath;
            await LoadSftpDirectoryAsync();
        }

        private async Task LoadSftpDirectoryAsync()
        {
            if (!int.TryParse(txtSftpPort.Text.Trim(), out int port)) port = 22;

            SetSftpBusy(true, $"Loading '{_sftpCurrentPath}'...");
            lstSftp.Items.Clear();
            _sftpEntries = Array.Empty<SftpFileEntry>();
            _sftpSelectedFile = null;
            UpdateSelectButton();

            try
            {
                _sftpEntries = await _sftpService.ListDirectoryAsync(
                    txtSftpHost.Text.Trim(), port,
                    txtSftpUser.Text.Trim(), txtSftpPass.Text,
                    _sftpCurrentPath);

                lstSftp.BeginUpdate();
                foreach (var entry in _sftpEntries)
                {
                    string icon = entry.IsDirectory ? "📁" : "📄";
                    lstSftp.Items.Add($"{icon}  {entry.Name}");
                }
                lstSftp.EndUpdate();

                btnSftpUp.Enabled = _sftpCurrentPath != "/";
                lblPickStatus.Text = $"{_sftpEntries.Count} item(s) in '{_sftpCurrentPath}'.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"SFTP error:\n{ex.Message}", "SFTP Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblPickStatus.Text = "SFTP connection failed.";
            }
            finally
            {
                SetSftpBusy(false, "");
            }
        }

        private void lstSftp_SelectedIndexChanged(object? sender, EventArgs e)
        {
            int idx = lstSftp.SelectedIndex;
            if (idx < 0 || idx >= _sftpEntries.Count)
            {
                _sftpSelectedFile = null;
                UpdateSelectButton();
                return;
            }

            var entry = _sftpEntries[idx];
            if (!entry.IsDirectory)
            {
                _sftpSelectedFile = entry.FullPath;
                lblPickStatus.Text = $"Selected: {entry.FullPath}";
            }
            else
            {
                _sftpSelectedFile = null;
            }

            UpdateSelectButton();
        }

        private async void lstSftp_DoubleClick(object? sender, EventArgs e)
        {
            int idx = lstSftp.SelectedIndex;
            if (idx < 0 || idx >= _sftpEntries.Count) return;

            var entry = _sftpEntries[idx];
            if (entry.IsDirectory)
            {
                _sftpCurrentPath = entry.FullPath.TrimEnd('/') + "/";
                txtSftpPath.Text = _sftpCurrentPath;
                await LoadSftpDirectoryAsync();
            }
        }

        private void SetSftpBusy(bool busy, string message)
        {
            btnSftpConnect.Enabled = !busy;
            btnSftpUp.Enabled = !busy && _sftpCurrentPath != "/";
            lstSftp.Enabled = !busy;
            Cursor = busy ? Cursors.WaitCursor : Cursors.Default;
            if (!string.IsNullOrEmpty(message))
                lblPickStatus.Text = message;
        }

        private static string GetParentPath(string path)
        {
            var trimmed = path.TrimEnd('/');
            if (string.IsNullOrEmpty(trimmed)) return "/";
            int idx = trimmed.LastIndexOf('/');
            return idx <= 0 ? "/" : trimmed.Substring(0, idx + 1);
        }

        // ═══════════════════════════════════════════════════════════════════
        // AZURE TAB
        // ═══════════════════════════════════════════════════════════════════

        private async void btnAzureSignIn_Click(object sender, EventArgs e)
        {
            SetAzureBusy(true, "Signing in — a browser window will open...");
            try
            {
                await _azureAccountService.SignInAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Sign-in failed:\n{ex.Message}", "Sign-In Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblAzureStatus.Text = "Sign-in failed.";
                SetAzureBusy(false, "");
                return;
            }

            btnAzureSignIn.Text = "🔄 Sign in again";
            _sessionManager.Refresh(CacheLevel.Accounts);
            await LoadAzureAccountsAsync();
        }

        private async void btnAzureRefresh_Click(object sender, EventArgs e)
        {
            if (lstAzureAccounts.SelectedItem is StorageAccountInfo account)
            {
                if (lstAzureContainers.SelectedItem is string container)
                {
                    _sessionManager.Refresh(CacheLevel.Blobs, account.BlobEndpoint, container);
                    await LoadAzureBlobsAsync(account, container, string.Empty);
                }
                else
                {
                    _sessionManager.Refresh(CacheLevel.Containers, account.BlobEndpoint);
                    await LoadAzureContainersAsync(account);
                }
            }
            else
            {
                _sessionManager.Refresh(CacheLevel.Accounts);
                await LoadAzureAccountsAsync();
            }
        }

        private async Task LoadAzureAccountsAsync()
        {
            if (!_azureAccountService.IsSignedIn) return;

            lstAzureAccounts.Items.Clear();
            lstAzureContainers.Items.Clear();
            lstAzureBlobs.Items.Clear();
            _azureAccounts = Array.Empty<StorageAccountInfo>();
            UpdateSelectButton();

            var cached = _sessionManager.GetCachedAccounts();
            if (cached != null)
            {
                _azureAccounts = cached;
                lstAzureAccounts.BeginUpdate();
                lstAzureAccounts.Items.AddRange(_azureAccounts.Cast<object>().ToArray());
                lstAzureAccounts.EndUpdate();
                lblAzureStatus.Text = $"Signed in — {_azureAccounts.Count} account(s). (cached)";
                return;
            }

            SetAzureBusy(true, "Discovering storage accounts...");
            try
            {
                _azureAccounts = await _azureAccountService.ListStorageAccountsAsync();
                _sessionManager.CacheAccounts(_azureAccounts);
                lstAzureAccounts.BeginUpdate();
                lstAzureAccounts.Items.AddRange(_azureAccounts.Cast<object>().ToArray());
                lstAzureAccounts.EndUpdate();
                lblAzureStatus.Text = _azureAccounts.Count > 0
                    ? $"Signed in — {_azureAccounts.Count} account(s) found."
                    : "Signed in — no storage accounts found.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to discover accounts:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblAzureStatus.Text = "Failed to discover accounts.";
            }
            finally { SetAzureBusy(false, ""); }
        }

        private async void lstAzureAccounts_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (lstAzureAccounts.SelectedItem is not StorageAccountInfo account
                || !_azureAccountService.IsSignedIn) return;
            await LoadAzureContainersAsync(account);
        }

        private async Task LoadAzureContainersAsync(StorageAccountInfo account)
        {
            lstAzureContainers.Items.Clear();
            lstAzureBlobs.Items.Clear();
            _azureCurrentPrefix = string.Empty;
            UpdateSelectButton();

            var cached = _sessionManager.GetCachedContainers(account.BlobEndpoint);
            if (cached != null)
            {
                lstAzureContainers.BeginUpdate();
                lstAzureContainers.Items.AddRange(cached.Cast<object>().ToArray());
                lstAzureContainers.EndUpdate();
                lblPickStatus.Text = $"{lstAzureContainers.Items.Count} container(s). (cached)";
                return;
            }

            SetAzureBusy(true, $"Loading containers in '{account.Name}'...");
            try
            {
                var containers = await _azureBlobService.ListContainersAsync(account.BlobEndpoint);
                _sessionManager.CacheContainers(account.BlobEndpoint, containers);
                lstAzureContainers.BeginUpdate();
                lstAzureContainers.Items.AddRange(containers.Cast<object>().ToArray());
                lstAzureContainers.EndUpdate();
                lblPickStatus.Text = $"{lstAzureContainers.Items.Count} container(s) in '{account.Name}'.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to list containers:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblPickStatus.Text = "Failed to list containers.";
            }
            finally { SetAzureBusy(false, ""); }
        }

        private async void lstAzureContainers_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (lstAzureAccounts.SelectedItem is not StorageAccountInfo account
                || lstAzureContainers.SelectedItem is not string container
                || !_azureAccountService.IsSignedIn) return;

            _azureCurrentPrefix = string.Empty;
            await LoadAzureBlobsAsync(account, container, string.Empty);
        }

        private async Task LoadAzureBlobsAsync(StorageAccountInfo account, string container, string prefix)
        {
            lstAzureBlobs.Items.Clear();
            _azureCurrentPrefix = prefix;
            UpdateBlobsLabel();
            UpdateSelectButton();

            var cached = _sessionManager.GetCachedHierarchy(account.BlobEndpoint, container, prefix);
            if (cached.HasValue)
            {
                PopulateAzureBlobList(prefix, cached.Value.Prefixes, cached.Value.Blobs);
                lblPickStatus.Text = $"{lstAzureBlobs.Items.Count} item(s). (cached)";
                return;
            }

            SetAzureBusy(true, $"Loading '{(string.IsNullOrEmpty(prefix) ? container : prefix)}'...");
            try
            {
                var (prefixes, blobs) = await _azureBlobService.ListBlobsByHierarchyAsync(
                    account.BlobEndpoint, container,
                    string.IsNullOrEmpty(prefix) ? null : prefix);

                _sessionManager.CacheHierarchy(account.BlobEndpoint, container, prefix, prefixes, blobs);
                PopulateAzureBlobList(prefix, prefixes, blobs);
                lblPickStatus.Text = $"{prefixes.Count + blobs.Count} item(s) in '{(string.IsNullOrEmpty(prefix) ? container : prefix)}'.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to list blobs:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblPickStatus.Text = "Failed to list blobs.";
            }
            finally { SetAzureBusy(false, ""); }
        }

        private void PopulateAzureBlobList(string prefix,
            IReadOnlyList<string> prefixes, IReadOnlyList<string> blobs)
        {
            lstAzureBlobs.BeginUpdate();
            lstAzureBlobs.Items.Clear();

            if (!string.IsNullOrEmpty(prefix))
                lstAzureBlobs.Items.Add(new BlobItem { IsBack = true });

            foreach (var p in prefixes)
            {
                var segment = p.StartsWith(prefix, StringComparison.Ordinal) ? p[prefix.Length..] : p;
                lstAzureBlobs.Items.Add(new BlobItem { IsFolder = true, FullPath = p, Display = segment });
            }

            foreach (var b in blobs)
            {
                var name = b.StartsWith(prefix, StringComparison.Ordinal) ? b[prefix.Length..] : b;
                lstAzureBlobs.Items.Add(new BlobItem { IsFolder = false, FullPath = b, Display = name });
            }

            lstAzureBlobs.EndUpdate();
        }

        private void UpdateBlobsLabel() =>
            lblAzureBlobs.Text = string.IsNullOrEmpty(_azureCurrentPrefix)
                ? "Blobs"
                : $"Blobs / {_azureCurrentPrefix}";

        private void lstAzureBlobs_SelectedIndexChanged(object? sender, EventArgs e) =>
            UpdateSelectButton();

        private async void lstAzureBlobs_DoubleClick(object? sender, EventArgs e)
        {
            if (lstAzureAccounts.SelectedItem is not StorageAccountInfo account
                || lstAzureContainers.SelectedItem is not string container
                || lstAzureBlobs.SelectedItem is not BlobItem item) return;

            if (item.IsBack)
            {
                await LoadAzureBlobsAsync(account, container, GetParentPrefix(_azureCurrentPrefix));
            }
            else if (item.IsFolder)
            {
                await LoadAzureBlobsAsync(account, container, item.FullPath);
            }
            else
            {
                // Double-clicking a blob triggers the Select action.
                await CommitAzureBlobAsync(account, container, item);
            }
        }

        private static string GetParentPrefix(string prefix)
        {
            if (string.IsNullOrEmpty(prefix)) return string.Empty;
            var trimmed = prefix.TrimEnd('/');
            int idx = trimmed.LastIndexOf('/');
            return idx < 0 ? string.Empty : trimmed[..(idx + 1)];
        }

        private void SetAzureBusy(bool busy, string message)
        {
            btnAzureSignIn.Enabled      = !busy;
            btnAzureRefresh.Enabled     = !busy;
            lstAzureAccounts.Enabled    = !busy;
            lstAzureContainers.Enabled  = !busy;
            lstAzureBlobs.Enabled       = !busy;
            Cursor = busy ? Cursors.WaitCursor : Cursors.Default;
            if (!string.IsNullOrEmpty(message))
                lblPickStatus.Text = message;
        }

        // ═══════════════════════════════════════════════════════════════════
        // SELECT / CANCEL
        // ═══════════════════════════════════════════════════════════════════

        private void UpdateSelectButton()
        {
            bool canSelect = tabSources.SelectedTab switch
            {
                var t when t == tabLocal  => !string.IsNullOrEmpty(txtLocalPath.Text),
                var t when t == tabSftp   => _sftpSelectedFile != null,
                var t when t == tabAzure  => lstAzureBlobs.SelectedItem is BlobItem b && !b.IsFolder && !b.IsBack,
                _                         => false,
            };

            btnSelect.Enabled = canSelect;
        }

        private async void btnSelect_Click(object sender, EventArgs e)
        {
            if (tabSources.SelectedTab == tabLocal)
            {
                CommitLocalFile(txtLocalPath.Text);
            }
            else if (tabSources.SelectedTab == tabSftp)
            {
                await CommitSftpFileAsync();
            }
            else if (tabSources.SelectedTab == tabAzure)
            {
                if (lstAzureAccounts.SelectedItem is StorageAccountInfo account
                    && lstAzureContainers.SelectedItem is string container
                    && lstAzureBlobs.SelectedItem is BlobItem item
                    && !item.IsFolder && !item.IsBack)
                {
                    await CommitAzureBlobAsync(account, container, item);
                }
            }
        }

        private void CommitLocalFile(string filePath)
        {
            SelectedFilePath    = filePath;
            SelectedDisplayName = filePath;
            IsTempFile          = false;
            DialogResult        = DialogResult.OK;
            Close();
        }

        private async Task CommitSftpFileAsync()
        {
            if (_sftpSelectedFile == null) return;

            if (!int.TryParse(txtSftpPort.Text.Trim(), out int port)) port = 22;

            SetSftpBusy(true, $"Downloading '{_sftpSelectedFile}'...");
            try
            {
                string tempFile = await _sftpService.DownloadFileToTempAsync(
                    txtSftpHost.Text.Trim(), port,
                    txtSftpUser.Text.Trim(), txtSftpPass.Text,
                    _sftpSelectedFile);

                SelectedFilePath    = tempFile;
                SelectedDisplayName = $"sftp://{txtSftpHost.Text.Trim()}{_sftpSelectedFile}";
                IsTempFile          = true;
                DialogResult        = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"SFTP download failed:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblPickStatus.Text = "Download failed.";
                SetSftpBusy(false, "");
            }
        }

        private async Task CommitAzureBlobAsync(
            StorageAccountInfo account, string container, BlobItem item)
        {
            SetAzureBusy(true, $"Downloading '{item.Display}'...");
            try
            {
                string tempFile = await _azureBlobService.DownloadBlobToTempFileAsync(
                    account.BlobEndpoint, container, item.FullPath);

                SelectedFilePath    = tempFile;
                SelectedDisplayName = $"{account.Name}/{container}/{item.FullPath}";
                IsTempFile          = true;
                DialogResult        = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to download blob:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblPickStatus.Text = "Download failed.";
                SetAzureBusy(false, "");
            }
        }

        private void btnCancelPick_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
