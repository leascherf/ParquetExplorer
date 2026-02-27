using System.Data;

namespace ParquetExplorer.Models
{
    /// <summary>
    /// Represents one page of data returned by <see cref="Services.ExplorerService"/>.
    /// </summary>
    public class PageResult
    {
        /// <summary>Rows for the current page (already cloned into a new DataTable).</summary>
        public DataTable PageTable { get; init; } = new();

        /// <summary>1-based current page index.</summary>
        public int CurrentPage { get; init; }

        /// <summary>Total number of pages given the current filter and page size.</summary>
        public int TotalPages { get; init; }

        /// <summary>Number of rows that match the current filter.</summary>
        public int FilteredRowCount { get; init; }

        /// <summary>Total rows in the loaded file (unfiltered).</summary>
        public int TotalRowCount { get; init; }
    }
}
