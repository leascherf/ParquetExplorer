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
    public partial class CompareForm : Form
    {
        private DataTable _leftTable = new();
        private DataTable _rightTable = new();

        private static readonly Color ColorDifferent = Color.FromArgb(255, 255, 180);   // yellow
        private static readonly Color ColorLeftOnly  = Color.FromArgb(255, 182, 182);   // pink/red
        private static readonly Color ColorRightOnly = Color.FromArgb(182, 255, 182);   // light green
        private static readonly Color ColorSame      = Color.White;

        public CompareForm()
        {
            InitializeComponent();
        }

        private async void btnOpenLeft_Click(object sender, EventArgs e)
        {
            var path = PickFile();
            if (path == null) return;
            lblLeftFile.Text = path;
            try { _leftTable = await LoadParquetTableAsync(path); }
            catch (Exception ex) { MessageBox.Show($"Error loading left file:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private async void btnOpenRight_Click(object sender, EventArgs e)
        {
            var path = PickFile();
            if (path == null) return;
            lblRightFile.Text = path;
            try { _rightTable = await LoadParquetTableAsync(path); }
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

        private static async Task<DataTable> LoadParquetTableAsync(string filePath)
        {
            var table = new DataTable();

            using var fileStream = File.OpenRead(filePath);
            using var reader = await ParquetReader.CreateAsync(fileStream);
            var schema = reader.Schema;

            foreach (var field in schema.Fields)
            {
                if (field is DataField df)
                {
                    var col = new System.Data.DataColumn(df.Name, Nullable.GetUnderlyingType(df.ClrType) ?? df.ClrType);
                    col.AllowDBNull = true;
                    table.Columns.Add(col);
                }
            }

            for (int rg = 0; rg < reader.RowGroupCount; rg++)
            {
                using var rowGroupReader = reader.OpenRowGroupReader(rg);
                var dataFields = schema.Fields.OfType<DataField>().ToArray();
                var columnData = new (DataField Field, System.Array Data)[dataFields.Length];

                for (int ci = 0; ci < dataFields.Length; ci++)
                {
                    var dc = await rowGroupReader.ReadColumnAsync(dataFields[ci]);
                    columnData[ci] = (dataFields[ci], (System.Array)dc.Data);
                }

                int rowCount = columnData.Length > 0 ? columnData[0].Data.Length : 0;
                for (int r = 0; r < rowCount; r++)
                {
                    var row = table.NewRow();
                    for (int ci = 0; ci < columnData.Length; ci++)
                    {
                        var val = columnData[ci].Data.GetValue(r);
                        row[ci] = val ?? DBNull.Value;
                    }
                    table.Rows.Add(row);
                }
            }

            return table;
        }

        private void btnCompare_Click(object sender, EventArgs e)
        {
            if (_leftTable.Columns.Count == 0)
            { MessageBox.Show("Please open the left file first.", "Missing File", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (_rightTable.Columns.Count == 0)
            { MessageBox.Show("Please open the right file first.", "Missing File", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            CompareAndDisplay();
        }

        private void CompareAndDisplay()
        {
            // Build key: row string representation for diff tracking
            var leftKeys  = BuildRowKeys(_leftTable);
            var rightKeys = BuildRowKeys(_rightTable);

            var leftKeySet  = new HashSet<string>(leftKeys);
            var rightKeySet = new HashSet<string>(rightKeys);

            // Determine common columns for comparison display
            var allColumns = _leftTable.Columns.Cast<System.Data.DataColumn>().Select(c => c.ColumnName)
                .Union(_rightTable.Columns.Cast<System.Data.DataColumn>().Select(c => c.ColumnName))
                .ToList();

            // Build display tables with row colors using merged schema
            var leftDisplay  = BuildDisplayTable(allColumns);
            var rightDisplay = BuildDisplayTable(allColumns);

            var leftColors  = new List<Color>();
            var rightColors = new List<Color>();

            // Align by index for positional diff (like WinMerge line-by-line)
            int maxRows = Math.Max(_leftTable.Rows.Count, _rightTable.Rows.Count);

            int sameCount = 0, diffCount = 0, leftOnlyCount = 0, rightOnlyCount = 0;

            for (int i = 0; i < maxRows; i++)
            {
                string? lKey = i < leftKeys.Count  ? leftKeys[i]  : null;
                string? rKey = i < rightKeys.Count ? rightKeys[i] : null;

                DataRow? lRow = i < _leftTable.Rows.Count  ? _leftTable.Rows[i]  : null;
                DataRow? rRow = i < _rightTable.Rows.Count ? _rightTable.Rows[i] : null;

                Color lColor, rColor;

                if (lKey == null)
                {
                    // Right only
                    lColor = ColorRightOnly; rColor = ColorRightOnly;
                    rightOnlyCount++;
                }
                else if (rKey == null)
                {
                    // Left only
                    lColor = ColorLeftOnly; rColor = ColorLeftOnly;
                    leftOnlyCount++;
                }
                else if (lKey == rKey)
                {
                    lColor = ColorSame; rColor = ColorSame;
                    sameCount++;
                }
                else
                {
                    lColor = ColorDifferent; rColor = ColorDifferent;
                    diffCount++;
                }

                AddDisplayRow(leftDisplay, allColumns, lRow);
                AddDisplayRow(rightDisplay, allColumns, rRow);
                leftColors.Add(lColor);
                rightColors.Add(rColor);
            }

            // Bind left grid
            dgvLeft.DataSource = leftDisplay;
            // Bind right grid
            dgvRight.DataSource = rightDisplay;

            // Color rows after binding
            Application.DoEvents();
            ColorGridRows(dgvLeft,  leftColors);
            ColorGridRows(dgvRight, rightColors);

            // Register paint events to keep colors
            dgvLeft.RowPrePaint  -= DgvLeft_RowPrePaint;
            dgvRight.RowPrePaint -= DgvRight_RowPrePaint;
            _leftColors  = leftColors;
            _rightColors = rightColors;
            dgvLeft.RowPrePaint  += DgvLeft_RowPrePaint;
            dgvRight.RowPrePaint += DgvRight_RowPrePaint;

            // Summary
            lblSummary.Text =
                $"Rows: Left={_leftTable.Rows.Count:N0}  Right={_rightTable.Rows.Count:N0}  " +
                $"Same={sameCount:N0}  Different={diffCount:N0}  " +
                $"Left-only={leftOnlyCount:N0}  Right-only={rightOnlyCount:N0}";
        }

        private List<Color> _leftColors  = new();
        private List<Color> _rightColors = new();

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

        private static List<string> BuildRowKeys(DataTable table)
        {
            var keys = new List<string>(table.Rows.Count);
            foreach (DataRow row in table.Rows)
                keys.Add(string.Join("|", row.ItemArray.Select(v => v == DBNull.Value ? "" : v?.ToString() ?? "")));
            return keys;
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
