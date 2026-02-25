using System.Data;
using ParquetExplorer.Models;

namespace ParquetExplorer.Services
{
    /// <summary>
    /// Encapsulates filtering and pagination logic for the parquet explorer.
    /// Has no dependency on Windows Forms and can be reused by any frontend.
    /// </summary>
    public class ExplorerService
    {
        private List<DataRow> _allRows      = new();
        private List<DataRow> _filteredRows = new();
        private DataTable     _dataTable    = new();

        public int CurrentPage { get; private set; } = 1;
        public int PageSize    { get; private set; } = 25;

        public int TotalRowCount    => _allRows.Count;
        public int FilteredRowCount => _filteredRows.Count;

        /// <summary>Column names available for the filter dropdown.</summary>
        public IReadOnlyList<string> ColumnNames =>
            _dataTable.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToList();

        /// <summary>
        /// Loads a Parquet file and resets filter / pagination state.
        /// </summary>
        public async Task LoadFileAsync(string filePath)
        {
            _dataTable    = await ParquetService.LoadAsync(filePath);
            _allRows      = _dataTable.Rows.Cast<DataRow>().ToList();
            _filteredRows = new List<DataRow>(_allRows);
            CurrentPage   = 1;
        }

        /// <summary>
        /// Filters rows. Pass <paramref name="columnName"/> = null to search all columns.
        /// Resets to page 1.
        /// </summary>
        public void ApplyFilter(string filterText, string? columnName)
        {
            if (string.IsNullOrEmpty(filterText))
            {
                _filteredRows = new List<DataRow>(_allRows);
            }
            else
            {
                bool allColumns = columnName == null;
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
                        var val = row[columnName!];
                        return val != null && val != DBNull.Value &&
                               val.ToString()!.IndexOf(filterText, StringComparison.OrdinalIgnoreCase) >= 0;
                    }
                }).ToList();
            }

            CurrentPage = 1;
        }

        /// <summary>Sets the page size and resets to page 1.</summary>
        public void SetPageSize(int pageSize)
        {
            PageSize    = pageSize;
            CurrentPage = 1;
        }

        public void NextPage()
        {
            int total = TotalPages();
            if (CurrentPage < total) CurrentPage++;
        }

        public void PreviousPage()
        {
            if (CurrentPage > 1) CurrentPage--;
        }

        public int TotalPages() =>
            Math.Max(1, (int)Math.Ceiling(_filteredRows.Count / (double)PageSize));

        /// <summary>
        /// Returns the data for the current page together with paging metadata.
        /// </summary>
        public PageResult GetPage()
        {
            int totalPages = TotalPages();
            if (CurrentPage > totalPages) CurrentPage = totalPages;

            var pageRows = _filteredRows
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            var pageTable = _dataTable.Clone();
            foreach (var row in pageRows)
                pageTable.ImportRow(row);

            return new PageResult
            {
                PageTable        = pageTable,
                CurrentPage      = CurrentPage,
                TotalPages       = totalPages,
                FilteredRowCount = _filteredRows.Count,
                TotalRowCount    = _allRows.Count,
            };
        }
    }
}
