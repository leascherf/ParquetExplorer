using System.Data;
using ParquetExplorer.Models;

namespace ParquetExplorer.Services
{
    /// <summary>
    /// Compares two Parquet DataTables and returns a <see cref="CompareResult"/>.
    /// Has no dependency on Windows Forms and can be reused by any frontend.
    /// </summary>
    public static class CompareService
    {
        public static CompareResult Compare(DataTable left, DataTable right)
        {
            var leftKeys  = BuildRowKeys(left);
            var rightKeys = BuildRowKeys(right);

            var allColumns = left.Columns.Cast<DataColumn>().Select(c => c.ColumnName)
                .Union(right.Columns.Cast<DataColumn>().Select(c => c.ColumnName))
                .ToList();

            int maxRows       = Math.Max(left.Rows.Count, right.Rows.Count);
            var rows          = new List<RowDiff>(maxRows);
            int sameCount     = 0;
            int diffCount     = 0;
            int leftOnly      = 0;
            int rightOnly     = 0;

            for (int i = 0; i < maxRows; i++)
            {
                string? lKey = i < leftKeys.Count  ? leftKeys[i]  : null;
                string? rKey = i < rightKeys.Count ? rightKeys[i] : null;

                DataRow? lRow = i < left.Rows.Count  ? left.Rows[i]  : null;
                DataRow? rRow = i < right.Rows.Count ? right.Rows[i] : null;

                DiffStatus status;
                if (lKey == null)       { status = DiffStatus.RightOnly; rightOnly++; }
                else if (rKey == null)  { status = DiffStatus.LeftOnly;  leftOnly++; }
                else if (lKey == rKey)  { status = DiffStatus.Same;      sameCount++; }
                else                    { status = DiffStatus.Different; diffCount++; }

                rows.Add(new RowDiff { LeftRow = lRow, RightRow = rRow, Status = status });
            }

            return new CompareResult
            {
                AllColumns    = allColumns,
                Rows          = rows,
                SameCount     = sameCount,
                DiffCount     = diffCount,
                LeftOnlyCount = leftOnly,
                RightOnlyCount= rightOnly,
                LeftTotalRows = left.Rows.Count,
                RightTotalRows= right.Rows.Count,
            };
        }

        private static List<string> BuildRowKeys(DataTable table)
        {
            var keys = new List<string>(table.Rows.Count);
            foreach (DataRow row in table.Rows)
                keys.Add(string.Join("|", row.ItemArray.Select(v => v == DBNull.Value ? "" : v?.ToString() ?? "")));
            return keys;
        }
    }
}
