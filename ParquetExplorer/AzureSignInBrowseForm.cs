using Azure.Core;
using ParquetExplorer.Models;
using ParquetExplorer.Services.Interfaces;

namespace ParquetExplorer
{
    /// <summary>
    /// Dialog that signs the user in with their Azure account,
    /// lists all accessible storage accounts, containers, and blobs,
    /// and lets them pick a blob to open.
    /// When a <paramref name="existingCredential"/> is supplied the sign-in step is
    /// skipped and the storage accounts are loaded automatically on startup.
    /// </summary>
    public partial class AzureSignInBrowseForm : Form
    {
        private readonly IAzureAccountService _azureAccountService;
        private readonly IAzureBlobService _azureBlobService;

        private TokenCredential? _credential;
        private IReadOnlyList<StorageAccountInfo> _storageAccounts = Array.Empty<StorageAccountInfo>();

        /// <summary>Local temp-file path of the downloaded blob (null if cancelled).</summary>
        public string? SelectedTempFilePath { get; private set; }

        /// <summary>Display name shown in the main form title bar (e.g. "account/container/blob").</summary>
        public string? SelectedBlobDisplayName { get; private set; }

        /// <summary>
        /// Creates a new dialog. Pass an <paramref name="existingCredential"/> to bypass the
        /// interactive sign-in step when the user has already authenticated in this session.
        /// </summary>
        public AzureSignInBrowseForm(IAzureAccountService azureAccountService, IAzureBlobService azureBlobService,
            TokenCredential? existingCredential = null)
        {
            _azureAccountService = azureAccountService;
            _azureBlobService = azureBlobService;
            _credential = existingCredential;
            InitializeComponent();
        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (_credential != null)
            {
                btnSignIn.Text = "🔄 Sign in again";
                lblSignInStatus.Text = "Already signed in — loading storage accounts...";
                await LoadStorageAccountsAsync();
            }
        }

        private async void btnSignIn_Click(object sender, EventArgs e)
        {
            SetBusy(true, "Signing in — a browser window will open...");

            TokenCredential? newCredential;
            try
            {
                newCredential = await _azureAccountService.SignInAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Sign-in failed:\n{ex.Message}", "Sign-In Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblSignInStatus.Text = "Sign-in failed.";
                SetBusy(false, "");
                return;
            }

            // Only replace the existing credential after the new one is obtained.
            _credential = newCredential;
            btnSignIn.Text = "🔄 Sign in again";
            await LoadStorageAccountsAsync();
        }

        private async Task LoadStorageAccountsAsync()
        {
            if (_credential == null) return;

            SetBusy(true, "Discovering storage accounts...");
            lstAccounts.Items.Clear();
            lstContainers.Items.Clear();
            lstBlobs.Items.Clear();
            btnOpen.Enabled = false;
            _storageAccounts = Array.Empty<StorageAccountInfo>();

            try
            {
                _storageAccounts = await _azureAccountService.ListStorageAccountsAsync(_credential);

                // Confirm the credential is working by caching it after first successful use.
                _azureAccountService.SetCachedCredential(_credential);

                foreach (var account in _storageAccounts)
                    lstAccounts.Items.Add(account);

                lblSignInStatus.Text = _storageAccounts.Count > 0
                    ? $"Signed in — {_storageAccounts.Count} storage account(s) found."
                    : "Signed in — no storage accounts found.";

                lblStatus.Text = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to discover accounts:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblSignInStatus.Text = "Failed to discover accounts.";
            }
            finally
            {
                SetBusy(false, "");
            }
        }

        private async void lstAccounts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstAccounts.SelectedItem is not StorageAccountInfo account || _credential == null) return;

            lstContainers.Items.Clear();
            lstBlobs.Items.Clear();
            btnOpen.Enabled = false;
            SetBusy(true, $"Loading containers in '{account.Name}'...");

            try
            {
                var containers = await _azureBlobService.ListContainersAsync(account.BlobEndpoint, _credential);
                foreach (var c in containers)
                    lstContainers.Items.Add(c);

                lblStatus.Text = $"{lstContainers.Items.Count} container(s) in '{account.Name}'.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to list containers:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Failed to list containers.";
            }
            finally
            {
                SetBusy(false, "");
            }
        }

        private async void lstContainers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstAccounts.SelectedItem is not StorageAccountInfo account
                || lstContainers.SelectedItem is not string container
                || _credential == null) return;

            lstBlobs.Items.Clear();
            btnOpen.Enabled = false;
            SetBusy(true, $"Loading blobs in '{container}'...");

            try
            {
                var blobs = await _azureBlobService.ListBlobsAsync(account.BlobEndpoint, _credential, container);
                foreach (var b in blobs)
                    lstBlobs.Items.Add(b);

                lblStatus.Text = $"{lstBlobs.Items.Count} blob(s) in '{container}'.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to list blobs:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Failed to list blobs.";
            }
            finally
            {
                SetBusy(false, "");
            }
        }

        private void lstBlobs_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnOpen.Enabled = lstBlobs.SelectedItem != null;
        }

        private async void btnOpen_Click(object sender, EventArgs e)
        {
            if (lstAccounts.SelectedItem is not StorageAccountInfo account
                || lstContainers.SelectedItem is not string container
                || lstBlobs.SelectedItem is not string blobName
                || _credential == null) return;

            SetBusy(true, $"Downloading '{blobName}'...");
            try
            {
                SelectedTempFilePath = await _azureBlobService.DownloadBlobToTempFileAsync(
                    account.BlobEndpoint, _credential, container, blobName);
                SelectedBlobDisplayName = $"{account.Name}/{container}/{blobName}";
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to download blob:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Download failed.";
            }
            finally
            {
                SetBusy(false, "");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void SetBusy(bool busy, string message)
        {
            btnSignIn.Enabled = !busy;
            lstAccounts.Enabled = !busy;
            lstContainers.Enabled = !busy;
            lstBlobs.Enabled = !busy;
            Cursor = busy ? Cursors.WaitCursor : Cursors.Default;
            if (!string.IsNullOrEmpty(message))
                lblStatus.Text = message;
        }
    }
}
