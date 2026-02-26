using Azure.Core;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ParquetExplorer.Services.Interfaces;

namespace ParquetExplorer.Services
{
    /// <summary>
    /// Azure Blob Storage implementation of <see cref="IAzureBlobService"/>.
    /// </summary>
    public class AzureBlobService : IAzureBlobService
    {
        // ── Connection-string overloads ──────────────────────────────────────

        public async Task<IReadOnlyList<string>> ListContainersAsync(string connectionString)
        {
            var client = new BlobServiceClient(connectionString);
            var containers = new List<string>();
            await foreach (var container in client.GetBlobContainersAsync())
                containers.Add(container.Name);
            return containers;
        }

        public async Task<IReadOnlyList<string>> ListBlobsAsync(string connectionString, string containerName, string? prefix = null)
        {
            var containerClient = new BlobContainerClient(connectionString, containerName);
            var blobs = new List<string>();
            await foreach (var blob in containerClient.GetBlobsAsync(prefix: prefix))
                blobs.Add(blob.Name);
            return blobs;
        }

        public async Task<string> DownloadBlobToTempFileAsync(string connectionString, string containerName, string blobName)
        {
            var containerClient = new BlobContainerClient(connectionString, containerName);
            var blobClient = containerClient.GetBlobClient(blobName);

            string extension = Path.GetExtension(blobName);
            string tempFile = Path.Combine(Path.GetTempPath(), $"parquetexplorer_{Guid.NewGuid()}{extension}");

            await blobClient.DownloadToAsync(tempFile);
            return tempFile;
        }

        // ── Credential-based overloads (Azure AD sign-in) ───────────────────

        public async Task<IReadOnlyList<string>> ListContainersAsync(Uri serviceUri, TokenCredential credential)
        {
            var client = new BlobServiceClient(serviceUri, credential);
            var containers = new List<string>();
            await foreach (var container in client.GetBlobContainersAsync())
                containers.Add(container.Name);
            return containers;
        }

        public async Task<IReadOnlyList<string>> ListBlobsAsync(Uri serviceUri, TokenCredential credential, string containerName, string? prefix = null)
        {
            var serviceClient = new BlobServiceClient(serviceUri, credential);
            var containerClient = serviceClient.GetBlobContainerClient(containerName);
            var blobs = new List<string>();
            await foreach (var blob in containerClient.GetBlobsAsync(prefix: prefix))
                blobs.Add(blob.Name);
            return blobs;
        }

        public async Task<string> DownloadBlobToTempFileAsync(Uri serviceUri, TokenCredential credential, string containerName, string blobName)
        {
            var serviceClient = new BlobServiceClient(serviceUri, credential);
            var containerClient = serviceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobName);

            string extension = Path.GetExtension(blobName);
            string tempFile = Path.Combine(Path.GetTempPath(), $"parquetexplorer_{Guid.NewGuid()}{extension}");

            await blobClient.DownloadToAsync(tempFile);
            return tempFile;
        }
    }
}
