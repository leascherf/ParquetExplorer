using ParquetExplorer.Models;

namespace ParquetExplorer.Services.Interfaces
{
    /// <summary>
    /// Abstraction for filtering and pagination logic of the parquet explorer.
    /// </summary>
    public interface IExplorerService
    {
        int CurrentPage { get; }
        int PageSize { get; }
        int TotalRowCount { get; }
        int FilteredRowCount { get; }
        IReadOnlyList<string> ColumnNames { get; }

        Task LoadFileAsync(string filePath);
        void ApplyFilter(string filterText, string? columnName);
        void SetPageSize(int pageSize);
        void NextPage();
        void PreviousPage();
        int TotalPages();
        PageResult GetPage();
    }
}
