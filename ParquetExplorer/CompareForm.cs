using System;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using ParquetExplorer.Models;
using ParquetExplorer.Services.Interfaces;
using System.Linq;
using System.Collections.Generic;

namespace ParquetExplorer
{
    public partial class CompareForm : Form
    {
        private readonly IParquetService _parquetService;
        private readonly ICompareService _compareService;

        private DataTable _leftTable = new();
        private DataTable _rightTable = new();

        private CompareResult? _lastResult;
        private List<RowDiff> _currentDisplayedDiffs = new();

        private static readonly Color ColorDifferent = Color.FromArgb(255, 255, 180);
        private static readonly Color ColorLeftOnly = Color.FromArgb(255, 182, 182);
        private readonly Color ColorRightOnly = Color.FromArgb(182, 255, 182);

        private int _currentPage = 1;
        private int _pageSize = 500;
        private int _totalPages = 1;
        private static readonly Color ColorSame = Color.White;

        private List<Color> _leftColors = new();
        private List<Color> _rightColors = new();

        private bool _isScrolling = false;

        public CompareForm(IParquetService parquetService, ICompareService compareService)
        {
            _parquetService = parquetService;
            _compareService = compareService;
            InitializeComponent();

            cmbFilter.Items.AddRange(new object[] { "All", "Different", "Left Only", "Right Only", "Same" });
            cmbFilter.SelectedIndex = 0;
            cmbFilter.SelectedIndexChanged += cmbFilter_SelectedIndexChanged;
        }

        private async void btnOpenLeft_Click(object sender, EventArgs e)
        {
            var path = PickFile();
            if (path == null) return;
            lblLeftFile.Text = path;
            try { _leftTable = await _parquetService.LoadAsync(path); }
            catch (Exception ex) { MessageBox.Show($"Error loading left file:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private async void btnOpenRight_Click(object sender, EventArgs e)
        {
            var path = PickFile();
            if (path == null) return;
            lblRightFile.Text = path;
            try { _rightTable = await _parquetService.LoadAsync(path); }
            catch (Exception ex) { MessageBox.Show($"Error loading right file:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private static string? PickFile()
        {
            using var dlg = new OpenFileDialog
            {
                Title = "Open Parquet File",
                Filter = "Parquet Files (*.parquet)|*.parquet|All Files (*.*)|*.*"
            };
            return dlg.ShowDialog() == DialogResult.OK ? dlg.FileName : null;
        }

        private void btnCompare_Click(object sender, EventArgs e)
        {
            if (_leftTable.Columns.Count == 0 || _rightTable.Columns.Count == 0)
            {
                MessageBox.Show("Please open both files first.", "Missing File", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var leftCols = _leftTable.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToList();
            var rightCols = _rightTable.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToList();
            var rightColsSet = new HashSet<string>(rightCols);
            var commonCols = leftCols.Where(c => rightColsSet.Contains(c)).ToList();

            string? selectedKey = null;

            if (commonCols.Any())
            {
                using var form = new Form
                {
                    Text = "Select Match Key Column",
                    Size = new Size(350, 180),
                    StartPosition = FormStartPosition.CenterParent,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    MaximizeBox = false,
                    MinimizeBox = false
                };

                var lbl = new Label { Text = "Select a column to align differences (or leave blank for row-by-row):", Left = 15, Top = 15, Width = 300, Height = 40 };
                var cmb = new ComboBox { Left = 15, Top = 60, Width = 300, DropDownStyle = ComboBoxStyle.DropDownList };

                cmb.Items.Add("(No Key - Row by Row Sequential)");
                foreach (var c in commonCols) cmb.Items.Add(c);
                cmb.SelectedIndex = 0;

                var btnOk = new Button { Text = "OK", Left = 135, Width = 80, Top = 100, DialogResult = DialogResult.OK };
                var btnCancel = new Button { Text = "Cancel", Left = 235, Width = 80, Top = 100, DialogResult = DialogResult.Cancel };

                form.Controls.Add(lbl);
                form.Controls.Add(cmb);
                form.Controls.Add(btnOk);
                form.Controls.Add(btnCancel);
                form.AcceptButton = btnOk;
                form.CancelButton = btnCancel;

                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    if (cmb.SelectedIndex > 0)
                        selectedKey = cmb.SelectedItem?.ToString();
                }
                else
                {
                    return; // comparison cancelled
                }
            }

            _lastResult = _compareService.Compare(_leftTable, _rightTable, selectedKey);
            ApplyFilter();
        }

        private void cmbFilter_SelectedIndexChanged(object? sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            if (_lastResult == null) return;

            var filterText = cmbFilter.SelectedItem?.ToString() ?? "All";

            var filteredRows = _lastResult.Rows.Where(r =>
            {
                if (filterText == "All") return true;
                if (filterText == "Different" && r.Status == DiffStatus.Different) return true;
                if (filterText == "Left Only" && r.Status == DiffStatus.LeftOnly) return true;
                if (filterText == "Right Only" && r.Status == DiffStatus.RightOnly) return true;
                if (filterText == "Same" && r.Status == DiffStatus.Same) return true;
                return false;
            }).ToList();

            _currentDisplayedDiffs = filteredRows;

            _currentPage = 1;
            RenderCurrentPage();

            int total = _lastResult.Rows.Count;
            double matchPerc = total > 0 ? (double)_lastResult.SameCount / total * 100 : 0;
            double diffPerc = total > 0 ? (double)_lastResult.DiffCount / total * 100 : 0;
            double leftPerc = total > 0 ? (double)_lastResult.LeftOnlyCount / total * 100 : 0;
            double rightPerc = total > 0 ? (double)_lastResult.RightOnlyCount / total * 100 : 0;

            lblSummary.Text =
                $"Total Aligned Rows: {total:N0} (100%)\r\n" +
                $"✓ Matches (Same): {matchPerc:F2}% ({_lastResult.SameCount:N0})   |   ⚠️ Partials (Different): {diffPerc:F2}% ({_lastResult.DiffCount:N0})\r\n" +
                $"◁ Left Only: {leftPerc:F2}% ({_lastResult.LeftOnlyCount:N0})   |   ▷ Right Only: {rightPerc:F2}% ({_lastResult.RightOnlyCount:N0})";
        }

        private void RenderCurrentPage()
        {
            if (_currentDisplayedDiffs == null || _lastResult == null) return;

            int totalRows = _currentDisplayedDiffs.Count;
            _totalPages = (int)Math.Ceiling(totalRows / (double)_pageSize);
            if (_totalPages < 1) _totalPages = 1;
            if (_currentPage > _totalPages) _currentPage = _totalPages;

            var pageRows = _currentDisplayedDiffs
                .Skip((_currentPage - 1) * _pageSize)
                .Take(_pageSize)
                .ToList();

            var leftDisplay = BuildDisplayTable(_lastResult.AllColumns);
            var rightDisplay = BuildDisplayTable(_lastResult.AllColumns);

            _leftColors = new List<Color>(pageRows.Count);
            _rightColors = new List<Color>(pageRows.Count);

            foreach (var diff in pageRows)
            {
                Color color = diff.Status switch
                {
                    DiffStatus.Same => ColorSame,
                    DiffStatus.Different => ColorDifferent,
                    DiffStatus.LeftOnly => ColorLeftOnly,
                    DiffStatus.RightOnly => ColorRightOnly,
                    _ => ColorSame
                };

                AddDisplayRow(leftDisplay, _lastResult.AllColumns, diff.LeftRow);
                AddDisplayRow(rightDisplay, _lastResult.AllColumns, diff.RightRow);
                _leftColors.Add(color);
                _rightColors.Add(color);
            }

            dgvLeft.DataSource = leftDisplay;
            dgvRight.DataSource = rightDisplay;

            AdjustColumnWidths(dgvLeft);
            AdjustColumnWidths(dgvRight);
            UpdateColumnVisibility();

            lblPageInfo.Text = $"Page {_currentPage} of {_totalPages}  ({totalRows:N0} rows)";
            btnPrev.Enabled = _currentPage > 1;
            btnNext.Enabled = _currentPage < _totalPages;
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                RenderCurrentPage();
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (_currentPage < _totalPages)
            {
                _currentPage++;
                RenderCurrentPage();
            }
        }

        private void cmbPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (int.TryParse(cmbPageSize.SelectedItem?.ToString(), out int ps))
            {
                _pageSize = ps;
                _currentPage = 1;
                RenderCurrentPage();
            }
        }



        private void DgvLeft_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < _leftColors.Count && e.CellStyle != null)
                e.CellStyle.BackColor = _leftColors[e.RowIndex];
        }

        private void DgvRight_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < _rightColors.Count && e.CellStyle != null)
                e.CellStyle.BackColor = _rightColors[e.RowIndex];
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

        private void chkShowEmptyColumns_CheckedChanged(object? sender, EventArgs e)
        {
            UpdateColumnVisibility();
        }

        private void UpdateColumnVisibility()
        {
            if (dgvLeft.Columns.Count == 0 || dgvRight.Columns.Count == 0 || _lastResult == null) return;

            bool showEmpty = chkShowEmptyColumns.Checked;

            foreach (string colName in _lastResult.AllColumns)
            {
                if (!dgvLeft.Columns.Contains(colName) || !dgvRight.Columns.Contains(colName)) continue;

                if (showEmpty)
                {
                    dgvLeft.Columns[colName].Visible = true;
                    dgvRight.Columns[colName].Visible = true;
                    continue;
                }

                bool hasValue = false;
                foreach (var diff in _currentDisplayedDiffs)
                {
                    string leftVal = diff.LeftRow?.Table.Columns.Contains(colName) == true && diff.LeftRow[colName] != DBNull.Value ? diff.LeftRow[colName]?.ToString() ?? "" : "";
                    string rightVal = diff.RightRow?.Table.Columns.Contains(colName) == true && diff.RightRow[colName] != DBNull.Value ? diff.RightRow[colName]?.ToString() ?? "" : "";

                    if (!string.IsNullOrWhiteSpace(leftVal) || !string.IsNullOrWhiteSpace(rightVal))
                    {
                        hasValue = true;
                        break;
                    }
                }

                dgvLeft.Columns[colName].Visible = hasValue;
                dgvRight.Columns[colName].Visible = hasValue;
            }
        }

        private static DataTable BuildDisplayTable(List<string> columns)
        {
            var dt = new DataTable();
            foreach (var col in columns)
                dt.Columns.Add(col, typeof(string));
            return dt;
        }

        private static void AddDisplayRow(DataTable display, List<string> allColumns, DataRow? source)
        {
            var row = display.NewRow();
            if (source != null)
            {
                foreach (var col in allColumns)
                {
                    if (source.Table.Columns.Contains(col))
                    {
                        var val = source[col];
                        row[col] = val == DBNull.Value ? "" : val?.ToString() ?? "";
                    }
                    else row[col] = "";
                }
            }
            display.Rows.Add(row);
        }

        private void Dgv_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _currentDisplayedDiffs.Count) return;
            var diff = _currentDisplayedDiffs[e.RowIndex];
            if (diff.Status != DiffStatus.Different) return;

            ShowDiffDialog(diff);
        }

        private void ShowDiffDialog(RowDiff diff)
        {
            if (_lastResult == null) return;

            var form = new Form
            {
                Text = "Row Differences (Double click to close)",
                Size = new Size(600, 400),
                StartPosition = FormStartPosition.CenterParent,
                ShowIcon = false,
                MinimizeBox = false,
                MaximizeBox = false
            };

            var dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible = false,
                BorderStyle = BorderStyle.None,
                BackgroundColor = SystemColors.Window
            };

            dgv.CellDoubleClick += (s, args) => form.Close();

            dgv.Columns.Add("Col", "Column");
            dgv.Columns.Add("Left", "Left Value");
            dgv.Columns.Add("Right", "Right Value");

            bool hasDiffs = false;
            foreach (var col in _lastResult.AllColumns)
            {
                string leftVal = diff.LeftRow?.Table.Columns.Contains(col) == true && diff.LeftRow[col] != DBNull.Value ? diff.LeftRow[col]?.ToString() ?? "" : "";
                string rightVal = diff.RightRow?.Table.Columns.Contains(col) == true && diff.RightRow[col] != DBNull.Value ? diff.RightRow[col]?.ToString() ?? "" : "";

                if (leftVal != rightVal)
                {
                    hasDiffs = true;
                    var rowIndex = dgv.Rows.Add(col, leftVal, rightVal);
                    dgv.Rows[rowIndex].DefaultCellStyle.BackColor = ColorDifferent;
                }
            }

            if (!hasDiffs)
            {
                MessageBox.Show("No column differences found!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            form.Controls.Add(dgv);
            form.ShowDialog(this);
        }

        private void Dgv_CellToolTipTextNeeded(object? sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < _currentDisplayedDiffs.Count)
            {
                var diff = _currentDisplayedDiffs[e.RowIndex];
                if (diff.Status == DiffStatus.Different && _lastResult != null)
                {
                    if (e.ColumnIndex >= 0 && e.ColumnIndex < _lastResult.AllColumns.Count)
                    {
                        var col = _lastResult.AllColumns[e.ColumnIndex];
                        string leftVal = diff.LeftRow?.Table.Columns.Contains(col) == true && diff.LeftRow[col] != DBNull.Value ? diff.LeftRow[col]?.ToString() ?? "" : "";
                        string rightVal = diff.RightRow?.Table.Columns.Contains(col) == true && diff.RightRow[col] != DBNull.Value ? diff.RightRow[col]?.ToString() ?? "" : "";
                        if (leftVal != rightVal)
                        {
                            e.ToolTipText = $"Difference in '{col}':\nLeft:  {leftVal}\nRight: {rightVal}\n\n(Double-click row for full details)";
                        }
                        else
                        {
                            e.ToolTipText = "Double-click row to view all differences.";
                        }
                    }
                }
            }
        }

        private void Dgv_Scroll(object? sender, ScrollEventArgs e)
        {
            if (_isScrolling) return;

            DataGridView active = (DataGridView)sender!;
            DataGridView other = active == dgvLeft ? dgvRight : dgvLeft;

            _isScrolling = true;
            try
            {
                if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
                {
                    other.HorizontalScrollingOffset = active.HorizontalScrollingOffset;
                }
                else
                {
                    if (other.Rows.Count > 0 && active.FirstDisplayedScrollingRowIndex >= 0)
                    {
                        int targetIndex = Math.Min(active.FirstDisplayedScrollingRowIndex, other.Rows.Count - 1);
                        other.FirstDisplayedScrollingRowIndex = targetIndex;
                    }
                }
            }
            finally
            {
                _isScrolling = false;
            }
        }

        private bool _isSelecting = false;

        private void Dgv_SelectionChanged(object? sender, EventArgs e)
        {
            if (_isSelecting) return;

            var sourceGrid = sender as DataGridView;
            var targetGrid = sourceGrid == dgvLeft ? dgvRight : dgvLeft;

            if (sourceGrid == null || targetGrid == null || sourceGrid.CurrentRow == null) return;

            try
            {
                _isSelecting = true;

                int rowIndex = sourceGrid.CurrentRow.Index;
                if (rowIndex >= 0 && rowIndex < targetGrid.Rows.Count)
                {
                    targetGrid.ClearSelection();
                    targetGrid.Rows[rowIndex].Selected = true;

                    int colIndex = sourceGrid.CurrentCell?.ColumnIndex ?? -1;
                    if (colIndex >= 0 && colIndex < targetGrid.Columns.Count && targetGrid.Columns[colIndex].Visible)
                    {
                        targetGrid.CurrentCell = targetGrid.Rows[rowIndex].Cells[colIndex];
                    }
                }
            }
            catch
            {
                // Ignore any selection synchronization edge cases
            }
            finally
            {
                _isSelecting = false;
            }
        }
    }
}
