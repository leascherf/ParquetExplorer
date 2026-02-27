using System.Data;
using ParquetExplorer.Models;
using ParquetExplorer.Services.Interfaces;

namespace ParquetExplorer.Services
{
    /// <summary>
    /// Compares two Parquet DataTables and returns a <see cref="CompareResult"/>.
    /// Has no dependency on Windows Forms and can be reused by any frontend.
    /// </summary>
    public class CompareService : ICompareService
    {
        public CompareResult Compare(DataTable left, DataTable right, string? matchKeyColumn = null)
        {
            var allColumns = left.Columns.Cast<DataColumn>().Select(c => c.ColumnName)
                .Union(right.Columns.Cast<DataColumn>().Select(c => c.ColumnName))
                .ToList();

            var rows = new List<RowDiff>();
            int sameCount = 0;
            int diffCount = 0;
            int leftOnly = 0;
            int rightOnly = 0;

            if (string.IsNullOrWhiteSpace(matchKeyColumn))
            {
                // Sequential fallback logic
                var leftKeys = BuildRowKeys(left);
                var rightKeys = BuildRowKeys(right);
                int maxRows = Math.Max(left.Rows.Count, right.Rows.Count);

                for (int i = 0; i < maxRows; i++)
                {
                    string? lKey = i < leftKeys.Count ? leftKeys[i] : null;
                    string? rKey = i < rightKeys.Count ? rightKeys[i] : null;

                    DataRow? lRow = i < left.Rows.Count ? left.Rows[i] : null;
                    DataRow? rRow = i < right.Rows.Count ? right.Rows[i] : null;

                    DiffStatus status;
                    if (lRow == null) { status = DiffStatus.RightOnly; rightOnly++; }
                    else if (rRow == null) { status = DiffStatus.LeftOnly; leftOnly++; }
                    else if (lKey == rKey) { status = DiffStatus.Same; sameCount++; }
                    else { status = DiffStatus.Different; diffCount++; }

                    rows.Add(new RowDiff { LeftRow = lRow, RightRow = rRow, Status = status });
                }
            }
            else
            {
                // ID-based matching algorithm
                var leftDict = new Dictionary<string, DataRow>();
                foreach (DataRow row in left.Rows)
                {
                    string key = row.Table.Columns.Contains(matchKeyColumn) && row[matchKeyColumn] != DBNull.Value ? row[matchKeyColumn]?.ToString() ?? "" : "";
                    if (!string.IsNullOrEmpty(key) && !leftDict.ContainsKey(key))
                        leftDict[key] = row;
                }

                var rightDict = new Dictionary<string, DataRow>();
                foreach (DataRow row in right.Rows)
                {
                    string key = row.Table.Columns.Contains(matchKeyColumn) && row[matchKeyColumn] != DBNull.Value ? row[matchKeyColumn]?.ToString() ?? "" : "";
                    if (!string.IsNullOrEmpty(key) && !rightDict.ContainsKey(key))
                        rightDict[key] = row;
                }

                var allKeys = leftDict.Keys.Union(rightDict.Keys).OrderBy(k => k).ToList();

                foreach (var key in allKeys)
                {
                    bool inLeft = leftDict.TryGetValue(key, out var lRow);
                    bool inRight = rightDict.TryGetValue(key, out var rRow);

                    DiffStatus status;
                    if (!inLeft) { status = DiffStatus.RightOnly; rightOnly++; }
                    else if (!inRight) { status = DiffStatus.LeftOnly; leftOnly++; }
                    else
                    {
                        string lStr = string.Join("|", lRow!.ItemArray.Select(v => v == DBNull.Value ? "" : v?.ToString() ?? ""));
                        string rStr = string.Join("|", rRow!.ItemArray.Select(v => v == DBNull.Value ? "" : v?.ToString() ?? ""));

                        if (lStr == rStr) { status = DiffStatus.Same; sameCount++; }
                        else { status = DiffStatus.Different; diffCount++; }
                    }

                    rows.Add(new RowDiff { LeftRow = lRow, RightRow = rRow, Status = status });
                }
            }

            return new CompareResult
            {
                AllColumns = allColumns,
                Rows = rows,
                SameCount = sameCount,
                DiffCount = diffCount,
                LeftOnlyCount = leftOnly,
                RightOnlyCount = rightOnly,
                LeftTotalRows = left.Rows.Count,
                RightTotalRows = right.Rows.Count,
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
