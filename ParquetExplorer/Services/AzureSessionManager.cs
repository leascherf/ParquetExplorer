using ParquetExplorer.Models;
using ParquetExplorer.Services.Interfaces;

namespace ParquetExplorer.Services
{
    /// <summary>
    /// In-memory, hierarchical cache for Azure storage data discovered during the
    /// current application session.  Registered as a singleton so the cached data
    /// persists across dialog openings without extra API round-trips.
    /// </summary>
    public class AzureSessionManager : IAzureSessionManager
    {
        private IReadOnlyList<StorageAccountInfo>? _accountsCache;
        private readonly Dictionary<Uri, IReadOnlyList<string>> _containersCache = new();
        private readonly Dictionary<string, IReadOnlyList<string>> _blobsCache = new();

        private static string BlobKey(Uri endpoint, string container) =>
            $"{endpoint.AbsoluteUri}|{container}";

        // ── Storage accounts ───────────────────────────────────────────────

        /// <inheritdoc/>
        public IReadOnlyList<StorageAccountInfo>? GetCachedAccounts() => _accountsCache;

        /// <inheritdoc/>
        public void CacheAccounts(IReadOnlyList<StorageAccountInfo> accounts) =>
            _accountsCache = accounts;

        // ── Containers ─────────────────────────────────────────────────────

        /// <inheritdoc/>
        public IReadOnlyList<string>? GetCachedContainers(Uri accountEndpoint) =>
            _containersCache.GetValueOrDefault(accountEndpoint);

        /// <inheritdoc/>
        public void CacheContainers(Uri accountEndpoint, IReadOnlyList<string> containers) =>
            _containersCache[accountEndpoint] = containers;

        // ── Blobs ──────────────────────────────────────────────────────────

        /// <inheritdoc/>
        public IReadOnlyList<string>? GetCachedBlobs(Uri accountEndpoint, string containerName) =>
            _blobsCache.GetValueOrDefault(BlobKey(accountEndpoint, containerName));

        /// <inheritdoc/>
        public void CacheBlobs(Uri accountEndpoint, string containerName, IReadOnlyList<string> blobs) =>
            _blobsCache[BlobKey(accountEndpoint, containerName)] = blobs;

        // ── Selective refresh ──────────────────────────────────────────────

        /// <inheritdoc/>
        public void Refresh(CacheLevel level, Uri? accountEndpoint = null, string? containerName = null)
        {
            switch (level)
            {
                case CacheLevel.Accounts:
                    // Full reset — re-discovering accounts means containers/blobs may also change.
                    _accountsCache = null;
                    _containersCache.Clear();
                    _blobsCache.Clear();
                    break;

                case CacheLevel.Containers:
                    if (accountEndpoint != null)
                    {
                        _containersCache.Remove(accountEndpoint);
                        // Also evict all blob entries that belong to this account.
                        var keysToRemove = _blobsCache.Keys
                            .Where(k => k.StartsWith(accountEndpoint.AbsoluteUri + "|", StringComparison.Ordinal))
                            .ToList();
                        foreach (var key in keysToRemove)
                            _blobsCache.Remove(key);
                    }
                    break;

                case CacheLevel.Blobs:
                    if (accountEndpoint != null && containerName != null)
                        _blobsCache.Remove(BlobKey(accountEndpoint, containerName));
                    break;
            }
        }
    }
}
