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
    /// Data is lazily loaded from <see cref="IAzureSessionManager"/> and written back
    /// after every successful API call.  Use the Refresh button to force a fresh fetch
    /// at the relevant cache level.
    /// </para>
    /// </summary>
    public partial class AzureExplorerPanel : UserControl
    {
        private readonly IAzureAccountService _azureAccountService;
        private readonly IAzureBlobService _azureBlobService;
        private readonly IAzureSessionManager _sessionManager;

        private IReadOnlyList<StorageAccountInfo> _storageAccounts = Array.Empty<StorageAccountInfo>();

        /// <summary>Raised when the user selects a blob and clicks "Open Blob".</summary>
        public event EventHandler<BlobSelectedEventArgs>? BlobOpenRequested;

        /// <summary>Raised when the user clicks the ✕ close button in the panel header.</summary>
        public event EventHandler? CloseRequested;

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
                    await LoadBlobsAsync(account, container);
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
            await LoadBlobsAsync(account, container);
        }

        private async Task LoadBlobsAsync(StorageAccountInfo account, string container)
        {
            if (!_azureAccountService.IsSignedIn) return;

            lstBlobs.Items.Clear();
            btnOpen.Enabled = false;

            // ── Lazy load ───────────────────────────────────────────────────
            var cached = _sessionManager.GetCachedBlobs(account.BlobEndpoint, container);
            if (cached != null)
            {
                lstBlobs.BeginUpdate();
                lstBlobs.Items.AddRange(cached.Cast<object>().ToArray());
                lstBlobs.EndUpdate();
                lblStatus.Text = $"{lstBlobs.Items.Count} blob(s). (cached)";
                return;
            }

            // ── Cache miss ──────────────────────────────────────────────────
            SetBusy(true, $"Loading blobs in '{container}'...");
            try
            {
                var blobs = await _azureBlobService.ListBlobsAsync(account.BlobEndpoint, container);
                _sessionManager.CacheBlobs(account.BlobEndpoint, container, blobs);
                lstBlobs.BeginUpdate();
                lstBlobs.Items.AddRange(blobs.Cast<object>().ToArray());
                lstBlobs.EndUpdate();
                lblStatus.Text = $"{lstBlobs.Items.Count} blob(s) in '{container}'.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to list blobs:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Failed to list blobs.";
            }
            finally { SetBusy(false, ""); }
        }

        private void lstBlobs_SelectedIndexChanged(object sender, EventArgs e) =>
            btnOpen.Enabled = lstBlobs.SelectedItem != null;

        private async void btnOpen_Click(object sender, EventArgs e)
        {
            if (lstAccounts.SelectedItem is not StorageAccountInfo account
                || lstContainers.SelectedItem is not string container
                || lstBlobs.SelectedItem is not string blobName
                || !_azureAccountService.IsSignedIn) return;

            SetBusy(true, $"Downloading '{blobName}'...");
            try
            {
                var tempFile = await _azureBlobService.DownloadBlobToTempFileAsync(
                    account.BlobEndpoint, container, blobName);
                BlobOpenRequested?.Invoke(this,
                    new BlobSelectedEventArgs(tempFile, $"{account.Name}/{container}/{blobName}"));
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

        private void SetBusy(bool busy, string message)
        {
            btnSignIn.Enabled = !busy;
            btnRefresh.Enabled = !busy;
            lstAccounts.Enabled = !busy;
            lstContainers.Enabled = !busy;
            lstBlobs.Enabled = !busy;
            btnOpen.Enabled = !busy && lstBlobs.SelectedItem != null;
            Cursor = busy ? Cursors.WaitCursor : Cursors.Default;
            if (!string.IsNullOrEmpty(message))
                lblStatus.Text = message;
        }
    }
}
