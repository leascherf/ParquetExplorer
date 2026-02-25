using System;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using ParquetExplorer.Models;
using ParquetExplorer.Services.Interfaces;

namespace ParquetExplorer
{
    public partial class CompareForm : Form
    {
        private readonly IParquetService _parquetService;
        private readonly ICompareService _compareService;

        private DataTable _leftTable  = new();
        private DataTable _rightTable = new();

        private static readonly Color ColorDifferent = Color.FromArgb(255, 255, 180);
        private static readonly Color ColorLeftOnly  = Color.FromArgb(255, 182, 182);
        private static readonly Color ColorRightOnly = Color.FromArgb(182, 255, 182);
        private static readonly Color ColorSame      = Color.White;

        private List<Color> _leftColors  = new();
        private List<Color> _rightColors = new();

        public CompareForm(IParquetService parquetService, ICompareService compareService)
        {
            _parquetService = parquetService;
            _compareService = compareService;
            InitializeComponent();
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
                Title  = "Open Parquet File",
                Filter = "Parquet Files (*.parquet)|*.parquet|All Files (*.*)|*.*"
            };
            return dlg.ShowDialog() == DialogResult.OK ? dlg.FileName : null;
        }

        private void btnCompare_Click(object sender, EventArgs e)
        {
            if (_leftTable.Columns.Count == 0)
            { MessageBox.Show("Please open the left file first.", "Missing File", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (_rightTable.Columns.Count == 0)
            { MessageBox.Show("Please open the right file first.", "Missing File", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            var result = _compareService.Compare(_leftTable, _rightTable);
            DisplayResult(result);
        }

        private void DisplayResult(CompareResult result)
        {
            var leftDisplay  = BuildDisplayTable(result.AllColumns);
            var rightDisplay = BuildDisplayTable(result.AllColumns);

            _leftColors  = new List<Color>(result.Rows.Count);
            _rightColors = new List<Color>(result.Rows.Count);

            foreach (var diff in result.Rows)
            {
                Color color = diff.Status switch
                {
                    DiffStatus.Same      => ColorSame,
                    DiffStatus.Different => ColorDifferent,
                    DiffStatus.LeftOnly  => ColorLeftOnly,
                    DiffStatus.RightOnly => ColorRightOnly,
                    _                    => ColorSame
                };

                AddDisplayRow(leftDisplay,  result.AllColumns, diff.LeftRow);
                AddDisplayRow(rightDisplay, result.AllColumns, diff.RightRow);
                _leftColors.Add(color);
                _rightColors.Add(color);
            }

            dgvLeft.DataSource  = leftDisplay;
            dgvRight.DataSource = rightDisplay;

            Application.DoEvents();
            ColorGridRows(dgvLeft,  _leftColors);
            ColorGridRows(dgvRight, _rightColors);

            dgvLeft.RowPrePaint  -= DgvLeft_RowPrePaint;
            dgvRight.RowPrePaint -= DgvRight_RowPrePaint;
            dgvLeft.RowPrePaint  += DgvLeft_RowPrePaint;
            dgvRight.RowPrePaint += DgvRight_RowPrePaint;

            lblSummary.Text =
                $"Rows: Left={result.LeftTotalRows:N0}  Right={result.RightTotalRows:N0}  " +
                $"Same={result.SameCount:N0}  Different={result.DiffCount:N0}  " +
                $"Left-only={result.LeftOnlyCount:N0}  Right-only={result.RightOnlyCount:N0}";
        }

        private void DgvLeft_RowPrePaint(object? sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < _leftColors.Count)
                dgvLeft.Rows[e.RowIndex].DefaultCellStyle.BackColor = _leftColors[e.RowIndex];
        }

        private void DgvRight_RowPrePaint(object? sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < _rightColors.Count)
                dgvRight.Rows[e.RowIndex].DefaultCellStyle.BackColor = _rightColors[e.RowIndex];
        }

        private static void ColorGridRows(DataGridView dgv, List<Color> colors)
        {
            for (int i = 0; i < dgv.Rows.Count && i < colors.Count; i++)
                dgv.Rows[i].DefaultCellStyle.BackColor = colors[i];
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
    }
}
