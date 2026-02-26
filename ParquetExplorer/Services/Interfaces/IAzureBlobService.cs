using Azure.Core;

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

        // ── Credential-based overloads (Azure AD sign-in) ──────────────────────

        /// <summary>
        /// Lists all containers in the storage account identified by <paramref name="serviceUri"/>
        /// using <paramref name="credential"/> for authentication.
        /// </summary>
        Task<IReadOnlyList<string>> ListContainersAsync(Uri serviceUri, TokenCredential credential);

        /// <summary>
        /// Lists all blobs in the specified container using <paramref name="credential"/>.
        /// </summary>
        Task<IReadOnlyList<string>> ListBlobsAsync(Uri serviceUri, TokenCredential credential, string containerName, string? prefix = null);

        /// <summary>
        /// Downloads a blob to a temporary local file using <paramref name="credential"/> and returns the path.
        /// </summary>
        Task<string> DownloadBlobToTempFileAsync(Uri serviceUri, TokenCredential credential, string containerName, string blobName);
    }
}
