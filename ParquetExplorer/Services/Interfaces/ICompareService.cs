using System.Data;
using ParquetExplorer.Models;

namespace ParquetExplorer.Services.Interfaces
{
    /// <summary>
    /// Abstraction for comparing two Parquet DataTables.
    /// </summary>
    public interface ICompareService
    {
        CompareResult Compare(DataTable left, DataTable right, string? matchKeyColumn = null);
    }
}
