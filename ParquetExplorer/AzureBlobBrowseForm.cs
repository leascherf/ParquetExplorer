using ParquetExplorer.Services.Interfaces;

namespace ParquetExplorer
{
    /// <summary>
    /// Dialog that lets the user connect to an Azure Storage account,
    /// browse containers and blobs, and pick a blob to open.
    /// </summary>
    public partial class AzureBlobBrowseForm : Form
    {
        private readonly IAzureBlobService _azureBlobService;

        /// <summary>
        /// After the user clicks OK, contains the local temp-file path of the
        /// downloaded blob (or null if the dialog was cancelled).
        /// </summary>
        public string? SelectedTempFilePath { get; private set; }

        /// <summary>
        /// The display name of the selected blob (container/blobName).
        /// </summary>
        public string? SelectedBlobDisplayName { get; private set; }

        public AzureBlobBrowseForm(IAzureBlobService azureBlobService)
        {
            _azureBlobService = azureBlobService;
            InitializeComponent();
        }

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            string connectionString = txtConnectionString.Text.Trim();
            if (string.IsNullOrEmpty(connectionString))
            {
                MessageBox.Show("Please enter a connection string.", "Missing Input",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SetBusy(true, "Connecting...");
            lstContainers.Items.Clear();
            lstBlobs.Items.Clear();
            btnOpen.Enabled = false;

            try
            {
                var containers = await _azureBlobService.ListContainersAsync(connectionString);
                foreach (var c in containers)
                    lstContainers.Items.Add(c);

                if (lstContainers.Items.Count == 0)
                    lblStatus.Text = "Connected — no containers found.";
                else
                    lblStatus.Text = $"Connected — {lstContainers.Items.Count} container(s) found.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to connect:\n{ex.Message}", "Connection Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Connection failed.";
            }
            finally
            {
                SetBusy(false, "");
            }
        }

        private async void lstContainers_SelectedIndexChanged(object sender, EventArgs e)
        {
            string? container = lstContainers.SelectedItem?.ToString();
            if (container == null) return;

            lstBlobs.Items.Clear();
            btnOpen.Enabled = false;
            SetBusy(true, $"Loading blobs in '{container}'...");

            try
            {
                string connectionString = txtConnectionString.Text.Trim();
                var blobs = await _azureBlobService.ListBlobsAsync(connectionString, container);
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
            string? container = lstContainers.SelectedItem?.ToString();
            string? blobName = lstBlobs.SelectedItem?.ToString();

            if (container == null || blobName == null) return;

            SetBusy(true, $"Downloading '{blobName}'...");
            try
            {
                string connectionString = txtConnectionString.Text.Trim();
                SelectedTempFilePath = await _azureBlobService.DownloadBlobToTempFileAsync(
                    connectionString, container, blobName);
                SelectedBlobDisplayName = $"{container}/{blobName}";
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
            btnConnect.Enabled = !busy;
            lstContainers.Enabled = !busy;
            lstBlobs.Enabled = !busy;
            Cursor = busy ? Cursors.WaitCursor : Cursors.Default;
            if (!string.IsNullOrEmpty(message))
                lblStatus.Text = message;
        }
    }
}
