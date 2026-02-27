using System.Data;

namespace ParquetExplorer.Services.Interfaces
{
    /// <summary>
    /// Abstraction for reading Parquet files. Decouples consumers from the
    /// concrete I/O implementation and enables easy testing/substitution.
    /// </summary>
    public interface IParquetService
    {
        Task<DataTable> LoadAsync(string filePath);
    }
}
