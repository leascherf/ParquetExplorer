using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Parquet;
using Parquet.Data;
using Parquet.Schema;

namespace ParquetExplorer
{
    public partial class MainForm : Form
    {
        private List<DataRow> _allRows = new();
        private List<DataRow> _filteredRows = new();
        private DataTable _dataTable = new();
        private int _currentPage = 1;
        private int _pageSize = 25;

        public MainForm()
        {
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
                Title = "Open Parquet File",
                Filter = "Parquet Files (*.parquet)|*.parquet|All Files (*.*)|*.*"
            };

            if (dlg.ShowDialog() != DialogResult.OK) return;

            lblStatus.Text = "Loading...";
            try
            {
                await LoadParquetFileAsync(dlg.FileName);
                lblFilePath.Text = dlg.FileName;
                lblStatus.Text = $"Loaded {_allRows.Count:N0} rows";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading file:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Error loading file";
            }
        }

        private async Task LoadParquetFileAsync(string filePath)
        {
            _dataTable = new DataTable();
            _allRows.Clear();
            _filteredRows.Clear();

            using var fileStream = File.OpenRead(filePath);
            using var reader = await ParquetReader.CreateAsync(fileStream);

            var schema = reader.Schema;

            // Build DataTable columns
            foreach (var field in schema.Fields)
            {
                if (field is DataField df)
                {
                    var col = new System.Data.DataColumn(df.Name, Nullable.GetUnderlyingType(df.ClrType) ?? df.ClrType);
                    col.AllowDBNull = true;
                    _dataTable.Columns.Add(col);
                }
            }

            // Read all row groups
            for (int rg = 0; rg < reader.RowGroupCount; rg++)
            {
                using var rowGroupReader = reader.OpenRowGroupReader(rg);
                var dataFields = schema.Fields.OfType<DataField>().ToArray();
                var columns = new DataColumn_Data[dataFields.Length];

                for (int ci = 0; ci < dataFields.Length; ci++)
                {
                    var dc = await rowGroupReader.ReadColumnAsync(dataFields[ci]);
                    columns[ci] = new DataColumn_Data { Field = dataFields[ci], Data = (System.Array)dc.Data };
                }

                int rowCount = columns.Length > 0 ? ((Array)columns[0].Data).Length : 0;

                for (int r = 0; r < rowCount; r++)
                {
                    var row = _dataTable.NewRow();
                    for (int ci = 0; ci < columns.Length; ci++)
                    {
                        var arr = (Array)columns[ci].Data;
                        var val = arr.GetValue(r);
                        row[ci] = val ?? DBNull.Value;
                    }
                    _dataTable.Rows.Add(row);
                    _allRows.Add(row);
                }
            }

            // Populate filter column dropdown
            cmbFilterColumn.Items.Clear();
            cmbFilterColumn.Items.Add("(All Columns)");
            foreach (System.Data.DataColumn col in _dataTable.Columns)
                cmbFilterColumn.Items.Add(col.ColumnName);
            cmbFilterColumn.SelectedIndex = 0;

            // Reset filter and pagination
            txtFilter.Text = string.Empty;
            _filteredRows = new List<DataRow>(_allRows);
            _currentPage = 1;
            ApplyPage();
        }

        private struct DataColumn_Data
        {
            public DataField Field;
            public System.Array Data;
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
            var filterText = txtFilter.Text.Trim();

            if (string.IsNullOrEmpty(filterText))
            {
                _filteredRows = new List<DataRow>(_allRows);
            }
            else
            {
                bool allColumns = cmbFilterColumn.SelectedIndex <= 0;
                string? selectedColumn = allColumns ? null : cmbFilterColumn.SelectedItem?.ToString();

                _filteredRows = _allRows.Where(row =>
                {
                    if (allColumns)
                    {
                        return row.ItemArray.Any(v =>
                            v != null && v != DBNull.Value &&
                            v.ToString()!.IndexOf(filterText, StringComparison.OrdinalIgnoreCase) >= 0);
                    }
                    else
                    {
                        var val = row[selectedColumn!];
                        return val != null && val != DBNull.Value &&
                               val.ToString()!.IndexOf(filterText, StringComparison.OrdinalIgnoreCase) >= 0;
                    }
                }).ToList();
            }

            _currentPage = 1;
            ApplyPage();
            lblStatus.Text = $"Showing {_filteredRows.Count:N0} of {_allRows.Count:N0} rows";
        }

        private void ApplyPage()
        {
            int totalPages = Math.Max(1, (int)Math.Ceiling(_filteredRows.Count / (double)_pageSize));
            if (_currentPage > totalPages) _currentPage = totalPages;

            var pageRows = _filteredRows
                .Skip((_currentPage - 1) * _pageSize)
                .Take(_pageSize)
                .ToList();

            var pageTable = _dataTable.Clone();
            foreach (var row in pageRows)
                pageTable.ImportRow(row);

            dataGridView1.DataSource = pageTable;

            lblPageInfo.Text = $"Page {_currentPage} of {totalPages}  ({_filteredRows.Count:N0} rows)";
            btnPrev.Enabled = _currentPage > 1;
            btnNext.Enabled = _currentPage < totalPages;
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (_currentPage > 1) { _currentPage--; ApplyPage(); }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            int totalPages = Math.Max(1, (int)Math.Ceiling(_filteredRows.Count / (double)_pageSize));
            if (_currentPage < totalPages) { _currentPage++; ApplyPage(); }
        }

        private void cmbPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (int.TryParse(cmbPageSize.SelectedItem?.ToString(), out int ps))
            {
                _pageSize = ps;
                _currentPage = 1;
                ApplyPage();
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
            var form = new CompareForm();
            form.Show(this);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
