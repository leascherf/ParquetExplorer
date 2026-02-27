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
        private readonly Dictionary<string, (IReadOnlyList<string> Prefixes, IReadOnlyList<string> Blobs)> _hierarchyCache = new();

        private static string BlobKey(Uri endpoint, string container) =>
            $"{endpoint.AbsoluteUri}|{container}";

        private static string HierarchyKey(Uri endpoint, string container, string prefix) =>
            $"{endpoint.AbsoluteUri}|{container}|{prefix}";

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

        // ── Hierarchical blob cache ────────────────────────────────────────

        /// <inheritdoc/>
        public (IReadOnlyList<string> Prefixes, IReadOnlyList<string> Blobs)? GetCachedHierarchy(
            Uri accountEndpoint, string containerName, string prefix) =>
            _hierarchyCache.TryGetValue(HierarchyKey(accountEndpoint, containerName, prefix), out var v) ? v : null;

        /// <inheritdoc/>
        public void CacheHierarchy(Uri accountEndpoint, string containerName, string prefix,
            IReadOnlyList<string> prefixes, IReadOnlyList<string> blobs) =>
            _hierarchyCache[HierarchyKey(accountEndpoint, containerName, prefix)] = (prefixes, blobs);

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
                    _hierarchyCache.Clear();
                    break;

                case CacheLevel.Containers:
                    if (accountEndpoint != null)
                    {
                        _containersCache.Remove(accountEndpoint);
                        // Also evict all blob and hierarchy entries that belong to this account.
                        var prefix = accountEndpoint.AbsoluteUri + "|";
                        RemoveByPrefix(_blobsCache, prefix);
                        RemoveByPrefix(_hierarchyCache, prefix);
                    }
                    break;

                case CacheLevel.Blobs:
                    if (accountEndpoint != null && containerName != null)
                    {
                        _blobsCache.Remove(BlobKey(accountEndpoint, containerName));
                        // Evict all hierarchy entries for this container (any prefix level).
                        var prefix = BlobKey(accountEndpoint, containerName) + "|";
                        RemoveByPrefix(_hierarchyCache, prefix);
                    }
                    break;
            }
        }

        private static void RemoveByPrefix<TValue>(Dictionary<string, TValue> dict, string keyPrefix)
        {
            var keysToRemove = dict.Keys
                .Where(k => k.StartsWith(keyPrefix, StringComparison.Ordinal))
                .ToList();
            foreach (var key in keysToRemove)
                dict.Remove(key);
        }
    }
}
