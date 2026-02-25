using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using ParquetExplorer.Services.Interfaces;

namespace ParquetExplorer
{
    public partial class MainForm : Form
    {
        private readonly IExplorerService _explorer;
        private readonly IParquetService _parquetService;
        private readonly ICompareService _compareService;
        private readonly IAzureBlobService _azureBlobService;

        public MainForm(IExplorerService explorer, IParquetService parquetService, ICompareService compareService, IAzureBlobService azureBlobService)
        {
            _explorer = explorer;
            _parquetService = parquetService;
            _compareService = compareService;
            _azureBlobService = azureBlobService;
            InitializeComponent();
            EnableDoubleBuffer(dataGridView1);
        }

        private static void EnableDoubleBuffer(DataGridView dgv)
        {
            var pi = typeof(DataGridView).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            pi?.SetValue(dgv, true, null);
        }

        private async void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await OpenFileAsync();
        }

        private async void btnOpen_Click(object sender, EventArgs e)
        {
            await OpenFileAsync();
        }

        private async Task OpenFileAsync()
        {
            using var dlg = new OpenFileDialog
            {
                Title = "Open Parquet File",
                Filter = "Parquet Files (*.parquet)|*.parquet|All Files (*.*)|*.*"
            };

            if (dlg.ShowDialog() != DialogResult.OK) return;

            toolStripStatusLabel1.Text = "Loading...";
            try
            {
                await _explorer.LoadFileAsync(dlg.FileName);

                var fileInfo = new System.IO.FileInfo(dlg.FileName);
                lblFilePath.Text = $"📄  {dlg.FileName}";
                lblFilePath.ForeColor = System.Drawing.Color.FromArgb(40, 56, 72);
                lblFilePath.Font = new System.Drawing.Font("Segoe UI", 9f);

                // Populate filter-column dropdown
                cmbFilterColumn.Items.Clear();
                cmbFilterColumn.Items.Add("(All Columns)");
                foreach (var col in _explorer.ColumnNames)
                    cmbFilterColumn.Items.Add(col);
                cmbFilterColumn.SelectedIndex = 0;

                txtFilter.Text = string.Empty;
                RefreshGrid();
                toolStripStatusLabel1.Text = $"✓  Loaded {_explorer.TotalRowCount:N0} rows  |  {_explorer.ColumnNames.Count} columns  |  {fileInfo.Length / 1024.0 / 1024.0:F2} MB";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading file:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                toolStripStatusLabel1.Text = "⚠  Error loading file";
            }
        }

        private async void openAzureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await OpenFromAzureAsync();
        }

        private async void btnOpenAzure_Click(object sender, EventArgs e)
        {
            await OpenFromAzureAsync();
        }

        private async Task OpenFromAzureAsync()
        {
            using var dlg = new AzureBlobBrowseForm(_azureBlobService);
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            string? tempFile = dlg.SelectedTempFilePath;
            string? displayName = dlg.SelectedBlobDisplayName;
            if (tempFile == null) return;

            toolStripStatusLabel1.Text = "Loading...";
            try
            {
                await _explorer.LoadFileAsync(tempFile);

                var fileInfo = new System.IO.FileInfo(tempFile);
                lblFilePath.Text = $"☁  {displayName}";
                lblFilePath.ForeColor = System.Drawing.Color.FromArgb(40, 56, 72);
                lblFilePath.Font = new System.Drawing.Font("Segoe UI", 9f);

                cmbFilterColumn.Items.Clear();
                cmbFilterColumn.Items.Add("(All Columns)");
                foreach (var col in _explorer.ColumnNames)
                    cmbFilterColumn.Items.Add(col);
                cmbFilterColumn.SelectedIndex = 0;

                txtFilter.Text = string.Empty;
                RefreshGrid();
                toolStripStatusLabel1.Text = $"✓  Loaded {_explorer.TotalRowCount:N0} rows  |  {_explorer.ColumnNames.Count} columns  |  {fileInfo.Length / 1024.0 / 1024.0:F2} MB";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading blob:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                toolStripStatusLabel1.Text = "⚠  Error loading blob";
            }
            finally
            {
                // Clean up the temporary file after loading
                try { if (System.IO.File.Exists(tempFile)) System.IO.File.Delete(tempFile); }
                catch { /* best effort */ }
            }
        }

        private void btnApplyFilter_Click(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void txtFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) ApplyFilter();
        }

        private void ApplyFilter()
        {
            bool allColumns = cmbFilterColumn.SelectedIndex <= 0;
            string? columnName = allColumns ? null : cmbFilterColumn.SelectedItem?.ToString();

            _explorer.ApplyFilter(txtFilter.Text.Trim(), columnName);
            RefreshGrid();
            toolStripStatusLabel1.Text = $"🔍  Showing {_explorer.FilteredRowCount:N0} of {_explorer.TotalRowCount:N0} rows";
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            _explorer.PreviousPage();
            RefreshGrid();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            _explorer.NextPage();
            RefreshGrid();
        }

        private void cmbPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (int.TryParse(cmbPageSize.SelectedItem?.ToString(), out int ps))
            {
                _explorer.SetPageSize(ps);
                RefreshGrid();
            }
        }

        private void RefreshGrid()
        {
            var page = _explorer.GetPage();
            dataGridView1.DataSource = page.PageTable;
            lblPageInfo.Text = $"Page {page.CurrentPage} of {page.TotalPages}  ({page.FilteredRowCount:N0} rows)";
            btnPrev.Enabled = page.CurrentPage > 1;
            btnNext.Enabled = page.CurrentPage < page.TotalPages;

            AdjustColumnWidths(dataGridView1);
            UpdateColumnVisibility();
        }

        private void btnShowEmptyColumns_Click(object sender, EventArgs e)
        {
            UpdateColumnVisibility();
        }

        private void UpdateColumnVisibility()
        {
            if (dataGridView1.Columns.Count == 0 || dataGridView1.DataSource == null) return;

            bool showEmpty = btnShowEmptyColumns.Checked;

            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                if (showEmpty)
                {
                    col.Visible = true;
                    continue;
                }

                bool hasValue = false;
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    string val = dataGridView1.Rows[i].Cells[col.Name].Value?.ToString() ?? "";
                    if (!string.IsNullOrWhiteSpace(val))
                    {
                        hasValue = true;
                        break;
                    }
                }

                col.Visible = hasValue;
            }
        }

        private void AdjustColumnWidths(DataGridView dgv)
        {
            if (dgv.Columns.Count == 0 || dgv.Rows.Count == 0) return;

            using var g = dgv.CreateGraphics();
            int maxWidth = (int)g.MeasureString(new string('W', 15), dgv.Font).Width + 20;

            foreach (DataGridViewColumn col in dgv.Columns)
            {
                int preferredWidth = col.GetPreferredWidth(DataGridViewAutoSizeColumnMode.DisplayedCells, true);
                int headerWidth = col.GetPreferredWidth(DataGridViewAutoSizeColumnMode.ColumnHeader, true);
                preferredWidth = Math.Max(preferredWidth, headerWidth);

                if (preferredWidth > maxWidth)
                    col.Width = maxWidth;
                else
                    col.Width = preferredWidth > 50 ? preferredWidth : 50;
            }
        }

        private void compareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenCompareForm();
        }

        private void btnCompare_Click(object sender, EventArgs e)
        {
            OpenCompareForm();
        }

        private void OpenCompareForm()
        {
            var form = new CompareForm(_parquetService, _compareService);
            form.Show(this);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
