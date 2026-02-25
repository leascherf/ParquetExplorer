using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using ParquetExplorer.Services.Interfaces;

namespace ParquetExplorer
{
    public partial class MainForm : Form
    {
        private readonly IExplorerService _explorer;
        private readonly IParquetService  _parquetService;
        private readonly ICompareService  _compareService;

        public MainForm(IExplorerService explorer, IParquetService parquetService, ICompareService compareService)
        {
            _explorer       = explorer;
            _parquetService = parquetService;
            _compareService = compareService;
            InitializeComponent();
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
                Title  = "Open Parquet File",
                Filter = "Parquet Files (*.parquet)|*.parquet|All Files (*.*)|*.*"
            };

            if (dlg.ShowDialog() != DialogResult.OK) return;

            lblStatus.Text = "Loading...";
            try
            {
                await _explorer.LoadFileAsync(dlg.FileName);

                lblFilePath.Text = dlg.FileName;

                // Populate filter-column dropdown
                cmbFilterColumn.Items.Clear();
                cmbFilterColumn.Items.Add("(All Columns)");
                foreach (var col in _explorer.ColumnNames)
                    cmbFilterColumn.Items.Add(col);
                cmbFilterColumn.SelectedIndex = 0;

                txtFilter.Text = string.Empty;
                RefreshGrid();
                lblStatus.Text = $"Loaded {_explorer.TotalRowCount:N0} rows";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading file:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Error loading file";
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
            bool allColumns    = cmbFilterColumn.SelectedIndex <= 0;
            string? columnName = allColumns ? null : cmbFilterColumn.SelectedItem?.ToString();

            _explorer.ApplyFilter(txtFilter.Text.Trim(), columnName);
            RefreshGrid();
            lblStatus.Text = $"Showing {_explorer.FilteredRowCount:N0} of {_explorer.TotalRowCount:N0} rows";
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
            btnPrev.Enabled  = page.CurrentPage > 1;
            btnNext.Enabled  = page.CurrentPage < page.TotalPages;
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
