using ParquetExplorer.Models;

namespace ParquetExplorer.Services.Interfaces
{
    /// <summary>Cache granularity used by <see cref="IAzureSessionManager.Refresh"/>.</summary>
    public enum CacheLevel
    {
        /// <summary>Invalidate the full storage-account list (and all descendent cache entries).</summary>
        Accounts,
        /// <summary>Invalidate the container list for a specific storage account.</summary>
        Containers,
        /// <summary>Invalidate the blob list for a specific container.</summary>
        Blobs,
    }

    /// <summary>
    /// Manages a hierarchical, in-memory cache of Azure storage data
    /// (accounts → containers → blobs) discovered during the current session.
    /// Registered as a singleton so data persists across dialog openings.
    /// </summary>
    public interface IAzureSessionManager
    {
        // ── Storage accounts ───────────────────────────────────────────────

        /// <summary>Returns the cached storage-account list, or <c>null</c> if not yet loaded.</summary>
        IReadOnlyList<StorageAccountInfo>? GetCachedAccounts();

        /// <summary>Stores the storage-account list in the cache.</summary>
        void CacheAccounts(IReadOnlyList<StorageAccountInfo> accounts);

        // ── Containers ─────────────────────────────────────────────────────

        /// <summary>
        /// Returns the cached container list for the given blob-service endpoint,
        /// or <c>null</c> if not yet loaded.
        /// </summary>
        IReadOnlyList<string>? GetCachedContainers(Uri accountEndpoint);

        /// <summary>Stores the container list for the given blob-service endpoint.</summary>
        void CacheContainers(Uri accountEndpoint, IReadOnlyList<string> containers);

        // ── Blobs ──────────────────────────────────────────────────────────

        /// <summary>
        /// Returns the cached blob list for the given endpoint + container,
        /// or <c>null</c> if not yet loaded.
        /// </summary>
        IReadOnlyList<string>? GetCachedBlobs(Uri accountEndpoint, string containerName);

        /// <summary>Stores the blob list for the given endpoint + container.</summary>
        void CacheBlobs(Uri accountEndpoint, string containerName, IReadOnlyList<string> blobs);

        // ── Selective refresh ──────────────────────────────────────────────

        /// <summary>
        /// Evicts cached data at the requested granularity so that the next access
        /// triggers a fresh API call.
        /// <list type="bullet">
        ///   <item><term><see cref="CacheLevel.Accounts"/></term>
        ///     <description>Clears everything (accounts, all containers, all blobs).</description></item>
        ///   <item><term><see cref="CacheLevel.Containers"/></term>
        ///     <description>Clears containers (and their blobs) for <paramref name="accountEndpoint"/>.</description></item>
        ///   <item><term><see cref="CacheLevel.Blobs"/></term>
        ///     <description>Clears blobs for <paramref name="accountEndpoint"/> + <paramref name="containerName"/>.</description></item>
        /// </list>
        /// </summary>
        void Refresh(CacheLevel level, Uri? accountEndpoint = null, string? containerName = null);
    }
}
