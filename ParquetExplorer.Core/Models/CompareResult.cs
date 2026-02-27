using System.Data;

namespace ParquetExplorer.Models
{
    public enum DiffStatus { Same, Different, LeftOnly, RightOnly }

    /// <summary>Diff information for a single aligned row pair.</summary>
    public class RowDiff
    {
        public DataRow? LeftRow  { get; init; }
        public DataRow? RightRow { get; init; }
        public DiffStatus Status { get; init; }
    }

    /// <summary>
    /// Full result of comparing two Parquet DataTables, returned by
    /// <see cref="Services.CompareService"/>. Contains no UI dependencies.
    /// </summary>
    public class CompareResult
    {
        /// <summary>Union of column names from both tables.</summary>
        public List<string> AllColumns { get; init; } = new();

        /// <summary>Aligned row pairs with their diff status.</summary>
        public List<RowDiff> Rows { get; init; } = new();

        public int SameCount      { get; init; }
        public int DiffCount      { get; init; }
        public int LeftOnlyCount  { get; init; }
        public int RightOnlyCount { get; init; }

        public int LeftTotalRows  { get; init; }
        public int RightTotalRows { get; init; }
    }
}
