namespace ParquetExplorer.Services.Interfaces
{
    /// <summary>
    /// Abstraction for Azure Blob Storage operations.
    /// Decouples consumers from the concrete Azure SDK implementation.
    /// <para>
    /// Azure AD (credential-based) overloads obtain the <c>TokenCredential</c> from
    /// <see cref="IAzureClientFactory"/> internally, so callers do not need to pass or
    /// store the credential themselves.
    /// </para>
    /// </summary>
    public interface IAzureBlobService
    {
        // ── Connection-string overloads ────────────────────────────────────────

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

        // ── Azure AD (factory-credential) overloads ────────────────────────────

        /// <summary>
        /// Lists all containers in the storage account identified by <paramref name="serviceUri"/>
        /// using the credential stored in <see cref="IAzureClientFactory"/>.
        /// </summary>
        Task<IReadOnlyList<string>> ListContainersAsync(Uri serviceUri);

        /// <summary>
        /// Lists all blobs in the specified container using the credential stored in
        /// <see cref="IAzureClientFactory"/>.
        /// </summary>
        Task<IReadOnlyList<string>> ListBlobsAsync(Uri serviceUri, string containerName, string? prefix = null);

        /// <summary>
        /// Downloads a blob to a temporary local file using the credential stored in
        /// <see cref="IAzureClientFactory"/> and returns the path.
        /// </summary>
        Task<string> DownloadBlobToTempFileAsync(Uri serviceUri, string containerName, string blobName);
    }
}
