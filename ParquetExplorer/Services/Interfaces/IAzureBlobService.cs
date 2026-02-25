namespace ParquetExplorer.Services.Interfaces
{
    /// <summary>
    /// Abstraction for Azure Blob Storage operations.
    /// Decouples consumers from the concrete Azure SDK implementation.
    /// </summary>
    public interface IAzureBlobService
    {
        /// <summary>
        /// Lists all containers in the storage account identified by <paramref name="connectionString"/>.
        /// </summary>
        Task<IReadOnlyList<string>> ListContainersAsync(string connectionString);

        /// <summary>
        /// Lists all blobs in the specified container, optionally filtered by <paramref name="prefix"/>.
        /// </summary>
        Task<IReadOnlyList<string>> ListBlobsAsync(string connectionString, string containerName, string? prefix = null);

        /// <summary>
        /// Downloads a blob to a temporary local file and returns its path.
        /// The caller is responsible for deleting the file when done.
        /// </summary>
        Task<string> DownloadBlobToTempFileAsync(string connectionString, string containerName, string blobName);
    }
}
