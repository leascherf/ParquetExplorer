using ParquetExplorer.Models;
using ParquetExplorer.Services.Interfaces;

namespace ParquetExplorer
{
    /// <summary>
    /// Event data for <see cref="AzureExplorerPanel.BlobOpenRequested"/>.
    /// </summary>
    public class BlobSelectedEventArgs : EventArgs
    {
        /// <summary>Local temp-file path of the downloaded blob.</summary>
        public string TempFilePath { get; }
        /// <summary>Display name shown in the main form title (e.g. "account/container/blob").</summary>
        public string DisplayName { get; }

        public BlobSelectedEventArgs(string tempFilePath, string displayName)
        {
            TempFilePath = tempFilePath;
            DisplayName = displayName;
        }
    }

    /// <summary>
    /// Collapsible left-panel UserControl that lets the user sign in with their Azure
    /// account, browse Storage Accounts → Containers → Blobs, and open a blob.
    /// <para>
    /// Authentication state is owned by the <see cref="IAzureClientFactory"/> singleton
    /// so closing/hiding this panel never disposes the global session.
    /// </para>
    /// <para>
    /// Blob browsing uses hierarchical listing (virtual-folder level by level) so that
    /// only the current directory level is fetched at a time, matching the speed of
    /// Azure Storage Explorer.  Results are cached per level; double-clicking a folder
    /// navigates into it, and the breadcrumb in the panel header reflects the path.
    /// </para>
    /// </summary>
    public partial class AzureExplorerPanel : UserControl
    {
        private readonly IAzureAccountService _azureAccountService;
        private readonly IAzureBlobService _azureBlobService;
        private readonly IAzureSessionManager _sessionManager;

        private IReadOnlyList<StorageAccountInfo> _storageAccounts = Array.Empty<StorageAccountInfo>();

        // ── Hierarchical blob navigation state ────────────────────────────
        /// <summary>
        /// Current virtual-folder prefix being browsed (e.g. "year=2024/month=01/").
        /// Empty string means the container root.
        /// </summary>
        private string _currentPrefix = string.Empty;

        /// <summary>Raised when the user selects a blob and clicks "Open Blob".</summary>
        public event EventHandler<BlobSelectedEventArgs>? BlobOpenRequested;

        /// <summary>Raised when the user clicks the ✕ close button in the panel header.</summary>
        public event EventHandler? CloseRequested;

        // ── Helper: item stored in lstBlobs ───────────────────────────────
        private sealed class BlobListItem
        {
            public bool   IsBack    { get; init; }
            public bool   IsFolder  { get; init; }
            /// <summary>Full blob name or full prefix path.</summary>
            public string FullPath  { get; init; } = "";
            /// <summary>Short display name (filename or last folder segment).</summary>
            public string DisplayName { get; init; } = "";

            public override string ToString() =>
                IsBack ? "📁  .." : IsFolder ? $"📁  {DisplayName}" : $"📄  {DisplayName}";
        }

        public AzureExplorerPanel(
            IAzureAccountService azureAccountService,
            IAzureBlobService azureBlobService,
            IAzureSessionManager sessionManager)
        {
            _azureAccountService = azureAccountService;
            _azureBlobService = azureBlobService;
            _sessionManager = sessionManager;
            InitializeComponent();
        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (_azureAccountService.IsSignedIn)
            {
                btnSignIn.Text = "🔄 Sign in again";
                lblSignInStatus.Text = "Already signed in — loading storage accounts...";
                await LoadStorageAccountsAsync();
            }
        }

        // ── Button handlers ────────────────────────────────────────────────

        private async void btnSignIn_Click(object sender, EventArgs e)
        {
            SetBusy(true, "Signing in — a browser window will open...");
            try
            {
                await _azureAccountService.SignInAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Sign-in failed:\n{ex.Message}", "Sign-In Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblSignInStatus.Text = "Sign-in failed.";
                SetBusy(false, "");
                return;
            }

            btnSignIn.Text = "🔄 Sign in again";
            _sessionManager.Refresh(CacheLevel.Accounts);
            await LoadStorageAccountsAsync();
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            if (lstAccounts.SelectedItem is StorageAccountInfo account)
            {
                if (lstContainers.SelectedItem is string container)
                {
                    _sessionManager.Refresh(CacheLevel.Blobs, account.BlobEndpoint, container);
                    await LoadBlobsHierarchyAsync(account, container, _currentPrefix);
                }
                else
                {
                    _sessionManager.Refresh(CacheLevel.Containers, account.BlobEndpoint);
                    await LoadContainersAsync(account);
                }
            }
            else
            {
                _sessionManager.Refresh(CacheLevel.Accounts);
                await LoadStorageAccountsAsync();
            }
        }

        private void btnClosePanel_Click(object sender, EventArgs e) =>
            CloseRequested?.Invoke(this, EventArgs.Empty);

        // ── Data loading ───────────────────────────────────────────────────

        private async Task LoadStorageAccountsAsync()
        {
            if (!_azureAccountService.IsSignedIn) return;

            lstAccounts.Items.Clear();
            lstContainers.Items.Clear();
            lstBlobs.Items.Clear();
            btnOpen.Enabled = false;
            _storageAccounts = Array.Empty<StorageAccountInfo>();

            // ── Lazy load: serve from cache when available ──────────────────
            var cached = _sessionManager.GetCachedAccounts();
            if (cached != null)
            {
                _storageAccounts = cached;
                lstAccounts.BeginUpdate();
                lstAccounts.Items.AddRange(_storageAccounts.Cast<object>().ToArray());
                lstAccounts.EndUpdate();
                lblSignInStatus.Text = $"Signed in — {_storageAccounts.Count} account(s). (cached)";
                lblStatus.Text = string.Empty;
                return;
            }

            // ── Cache miss: fetch from Azure ────────────────────────────────
            SetBusy(true, "Discovering storage accounts...");
            try
            {
                _storageAccounts = await _azureAccountService.ListStorageAccountsAsync();
                _sessionManager.CacheAccounts(_storageAccounts);
                lstAccounts.BeginUpdate();
                lstAccounts.Items.AddRange(_storageAccounts.Cast<object>().ToArray());
                lstAccounts.EndUpdate();
                lblSignInStatus.Text = _storageAccounts.Count > 0
                    ? $"Signed in — {_storageAccounts.Count} account(s) found."
                    : "Signed in — no storage accounts found.";
                lblStatus.Text = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to discover accounts:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblSignInStatus.Text = "Failed to discover accounts.";
            }
            finally { SetBusy(false, ""); }
        }

        private async void lstAccounts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstAccounts.SelectedItem is not StorageAccountInfo account
                || !_azureAccountService.IsSignedIn) return;
            await LoadContainersAsync(account);
        }

        private async Task LoadContainersAsync(StorageAccountInfo account)
        {
            if (!_azureAccountService.IsSignedIn) return;

            lstContainers.Items.Clear();
            lstBlobs.Items.Clear();
            btnOpen.Enabled = false;
            _currentPrefix = string.Empty;
            UpdateBlobsLabel();

            // ── Lazy load ───────────────────────────────────────────────────
            var cached = _sessionManager.GetCachedContainers(account.BlobEndpoint);
            if (cached != null)
            {
                lstContainers.BeginUpdate();
                lstContainers.Items.AddRange(cached.Cast<object>().ToArray());
                lstContainers.EndUpdate();
                lblStatus.Text = $"{lstContainers.Items.Count} container(s). (cached)";
                return;
            }

            // ── Cache miss ──────────────────────────────────────────────────
            SetBusy(true, $"Loading containers in '{account.Name}'...");
            try
            {
                var containers = await _azureBlobService.ListContainersAsync(account.BlobEndpoint);
                _sessionManager.CacheContainers(account.BlobEndpoint, containers);
                lstContainers.BeginUpdate();
                lstContainers.Items.AddRange(containers.Cast<object>().ToArray());
                lstContainers.EndUpdate();
                lblStatus.Text = $"{lstContainers.Items.Count} container(s) in '{account.Name}'.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to list containers:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Failed to list containers.";
            }
            finally { SetBusy(false, ""); }
        }

        private async void lstContainers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstAccounts.SelectedItem is not StorageAccountInfo account
                || lstContainers.SelectedItem is not string container
                || !_azureAccountService.IsSignedIn) return;

            // Reset to the root level when the user switches containers.
            _currentPrefix = string.Empty;
            await LoadBlobsHierarchyAsync(account, container, _currentPrefix);
        }

        /// <summary>
        /// Loads the hierarchical listing for <paramref name="prefix"/> inside
        /// <paramref name="container"/>.  Only the items at this virtual-folder level
        /// are fetched, making the operation fast even for very large containers.
        /// </summary>
        private async Task LoadBlobsHierarchyAsync(StorageAccountInfo account, string container, string prefix)
        {
            if (!_azureAccountService.IsSignedIn) return;

            lstBlobs.Items.Clear();
            btnOpen.Enabled = false;
            _currentPrefix = prefix;
            UpdateBlobsLabel();

            // ── Lazy load ───────────────────────────────────────────────────
            var cached = _sessionManager.GetCachedHierarchy(account.BlobEndpoint, container, prefix);
            if (cached.HasValue)
            {
                PopulateBlobList(prefix, cached.Value.Prefixes, cached.Value.Blobs);
                lblStatus.Text = $"{lstBlobs.Items.Count - (string.IsNullOrEmpty(prefix) ? 0 : 1)} item(s). (cached)";
                return;
            }

            // ── Cache miss ──────────────────────────────────────────────────
            SetBusy(true, $"Loading '{(string.IsNullOrEmpty(prefix) ? container : prefix)}'...");
            try
            {
                var (prefixes, blobs) = await _azureBlobService.ListBlobsByHierarchyAsync(
                    account.BlobEndpoint, container, string.IsNullOrEmpty(prefix) ? null : prefix);
                _sessionManager.CacheHierarchy(account.BlobEndpoint, container, prefix, prefixes, blobs);
                PopulateBlobList(prefix, prefixes, blobs);
                int itemCount = prefixes.Count + blobs.Count;
                lblStatus.Text = $"{itemCount} item(s) in '{(string.IsNullOrEmpty(prefix) ? container : prefix)}'.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to list blobs:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Failed to list blobs.";
            }
            finally { SetBusy(false, ""); }
        }

        /// <summary>
        /// Fills <see cref="lstBlobs"/> with a ".." back-item (when not at root),
        /// virtual-folder items, and blob items.
        /// </summary>
        private void PopulateBlobList(string prefix, IReadOnlyList<string> prefixes, IReadOnlyList<string> blobs)
        {
            lstBlobs.BeginUpdate();
            lstBlobs.Items.Clear();

            // Back-navigation item when inside a virtual folder.
            if (!string.IsNullOrEmpty(prefix))
                lstBlobs.Items.Add(new BlobListItem { IsBack = true });

            // Virtual folders — show only the last segment (e.g. "month=01/").
            foreach (var p in prefixes)
            {
                var segment = p.StartsWith(prefix, StringComparison.Ordinal) ? p[prefix.Length..] : p;
                lstBlobs.Items.Add(new BlobListItem
                {
                    IsFolder    = true,
                    FullPath    = p,
                    DisplayName = segment,
                });
            }

            // Actual blobs — show only the filename.
            foreach (var b in blobs)
            {
                var name = b.StartsWith(prefix, StringComparison.Ordinal) ? b[prefix.Length..] : b;
                lstBlobs.Items.Add(new BlobListItem
                {
                    IsFolder    = false,
                    FullPath    = b,
                    DisplayName = name,
                });
            }

            lstBlobs.EndUpdate();
        }

        private void lstBlobs_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Enable Open only when a real blob (not a folder or back-item) is selected.
            btnOpen.Enabled = lstBlobs.SelectedItem is BlobListItem item
                && !item.IsFolder && !item.IsBack;
        }

        private async void lstBlobs_DoubleClick(object sender, EventArgs e)
        {
            if (lstAccounts.SelectedItem is not StorageAccountInfo account
                || lstContainers.SelectedItem is not string container
                || lstBlobs.SelectedItem is not BlobListItem item
                || !_azureAccountService.IsSignedIn) return;

            if (item.IsBack)
            {
                // Navigate up one level.
                await LoadBlobsHierarchyAsync(account, container, GetParentPrefix(_currentPrefix));
            }
            else if (item.IsFolder)
            {
                // Navigate into the selected virtual folder.
                await LoadBlobsHierarchyAsync(account, container, item.FullPath);
            }
            else
            {
                // Double-clicking a blob is a shortcut for the Open button.
                await OpenSelectedBlobAsync(account, container, item);
            }
        }

        private async void btnOpen_Click(object sender, EventArgs e)
        {
            if (lstAccounts.SelectedItem is not StorageAccountInfo account
                || lstContainers.SelectedItem is not string container
                || lstBlobs.SelectedItem is not BlobListItem item
                || item.IsFolder || item.IsBack
                || !_azureAccountService.IsSignedIn) return;

            await OpenSelectedBlobAsync(account, container, item);
        }

        private async Task OpenSelectedBlobAsync(StorageAccountInfo account, string container, BlobListItem item)
        {
            SetBusy(true, $"Downloading '{item.DisplayName}'...");
            try
            {
                var tempFile = await _azureBlobService.DownloadBlobToTempFileAsync(
                    account.BlobEndpoint, container, item.FullPath);
                BlobOpenRequested?.Invoke(this,
                    new BlobSelectedEventArgs(tempFile, $"{account.Name}/{container}/{item.FullPath}"));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to download blob:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Download failed.";
            }
            finally { SetBusy(false, ""); }
        }

        // ── Helpers ────────────────────────────────────────────────────────

        /// <summary>
        /// Updates <see cref="lblBlobsLabel"/> to show "Blobs" at root or
        /// "Blobs / current/prefix/" when inside a virtual folder.
        /// </summary>
        private void UpdateBlobsLabel() =>
            lblBlobsLabel.Text = string.IsNullOrEmpty(_currentPrefix)
                ? "Blobs"
                : $"Blobs / {_currentPrefix}";

        /// <summary>
        /// Returns the parent prefix of <paramref name="prefix"/>.
        /// E.g. "year=2024/month=01/" → "year=2024/", "year=2024/" → "".
        /// </summary>
        private static string GetParentPrefix(string prefix)
        {
            if (string.IsNullOrEmpty(prefix)) return string.Empty;
            var trimmed = prefix.TrimEnd('/');
            var idx = trimmed.LastIndexOf('/');
            return idx < 0 ? string.Empty : trimmed.Substring(0, idx + 1);
        }

        private void SetBusy(bool busy, string message)
        {
            btnSignIn.Enabled = !busy;
            btnRefresh.Enabled = !busy;
            lstAccounts.Enabled = !busy;
            lstContainers.Enabled = !busy;
            lstBlobs.Enabled = !busy;
            btnOpen.Enabled = !busy && lstBlobs.SelectedItem is BlobListItem item && !item.IsFolder && !item.IsBack;
            Cursor = busy ? Cursors.WaitCursor : Cursors.Default;
            if (!string.IsNullOrEmpty(message))
                lblStatus.Text = message;
        }
    }
}
