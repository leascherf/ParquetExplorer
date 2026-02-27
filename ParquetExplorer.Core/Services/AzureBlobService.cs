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
        private readonly IAzureClientFactory _clientFactory;

        public AzureBlobService(IAzureClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        // ── Connection-string overloads ──────────────────────────────────────

        public async Task<IReadOnlyList<string>> ListContainersAsync(string connectionString)
        {
            var client = new BlobServiceClient(connectionString);
            var containers = new List<string>();
            await foreach (var container in client.GetBlobContainersAsync().ConfigureAwait(false))
                containers.Add(container.Name);
            return containers;
        }

        public async Task<IReadOnlyList<string>> ListBlobsAsync(string connectionString, string containerName, string? prefix = null)
        {
            var containerClient = new BlobContainerClient(connectionString, containerName);
            var blobs = new List<string>();
            await foreach (var blob in containerClient.GetBlobsAsync(prefix: prefix).ConfigureAwait(false))
                blobs.Add(blob.Name);
            return blobs;
        }

        public async Task<string> DownloadBlobToTempFileAsync(string connectionString, string containerName, string blobName)
        {
            var containerClient = new BlobContainerClient(connectionString, containerName);
            var blobClient = containerClient.GetBlobClient(blobName);

            string extension = Path.GetExtension(blobName);
            string tempFile = Path.Combine(Path.GetTempPath(), $"parquetexplorer_{Guid.NewGuid()}{extension}");

            await blobClient.DownloadToAsync(tempFile).ConfigureAwait(false);
            return tempFile;
        }

        public async Task<(IReadOnlyList<string> Prefixes, IReadOnlyList<string> Blobs)> ListBlobsByHierarchyAsync(
            string connectionString, string containerName, string? prefix = null)
        {
            var containerClient = new BlobContainerClient(connectionString, containerName);
            var prefixes = new List<string>();
            var blobs = new List<string>();
            await foreach (var item in containerClient
                .GetBlobsByHierarchyAsync(delimiter: "/", prefix: prefix)
                .ConfigureAwait(false))
            {
                if (item.IsPrefix)
                    prefixes.Add(item.Prefix);
                else
                    blobs.Add(item.Blob.Name);
            }
            return (prefixes, blobs);
        }

        // ── Azure AD (factory-credential) overloads ──────────────────────────
        // The BlobServiceClient is created via IAzureClientFactory, which holds the
        // same TokenCredential as the ArmClient used for account discovery.  This
        // ensures MSAL's token cache is shared and no second browser prompt is needed.

        public async Task<IReadOnlyList<string>> ListContainersAsync(Uri serviceUri)
        {
            var client = _clientFactory.CreateBlobServiceClient(serviceUri);
            var containers = new List<string>();
            await foreach (var container in client.GetBlobContainersAsync().ConfigureAwait(false))
                containers.Add(container.Name);
            return containers;
        }

        public async Task<IReadOnlyList<string>> ListBlobsAsync(Uri serviceUri, string containerName, string? prefix = null)
        {
            var serviceClient = _clientFactory.CreateBlobServiceClient(serviceUri);
            var containerClient = serviceClient.GetBlobContainerClient(containerName);
            var blobs = new List<string>();
            await foreach (var blob in containerClient.GetBlobsAsync(prefix: prefix).ConfigureAwait(false))
                blobs.Add(blob.Name);
            return blobs;
        }

        public async Task<string> DownloadBlobToTempFileAsync(Uri serviceUri, string containerName, string blobName)
        {
            var serviceClient = _clientFactory.CreateBlobServiceClient(serviceUri);
            var containerClient = serviceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobName);

            string extension = Path.GetExtension(blobName);
            string tempFile = Path.Combine(Path.GetTempPath(), $"parquetexplorer_{Guid.NewGuid()}{extension}");

            await blobClient.DownloadToAsync(tempFile).ConfigureAwait(false);
            return tempFile;
        }

        public async Task<(IReadOnlyList<string> Prefixes, IReadOnlyList<string> Blobs)> ListBlobsByHierarchyAsync(
            Uri serviceUri, string containerName, string? prefix = null)
        {
            var serviceClient = _clientFactory.CreateBlobServiceClient(serviceUri);
            var containerClient = serviceClient.GetBlobContainerClient(containerName);
            var prefixes = new List<string>();
            var blobs = new List<string>();
            await foreach (var item in containerClient
                .GetBlobsByHierarchyAsync(delimiter: "/", prefix: prefix)
                .ConfigureAwait(false))
            {
                if (item.IsPrefix)
                    prefixes.Add(item.Prefix);
                else
                    blobs.Add(item.Blob.Name);
            }
            return (prefixes, blobs);
        }
    }
}
