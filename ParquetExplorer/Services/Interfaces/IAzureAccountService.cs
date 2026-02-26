using ParquetExplorer.Models;

namespace ParquetExplorer.Services.Interfaces
{
    /// <summary>
    /// Abstraction for Azure AD authentication and storage account discovery.
    /// Credential management (storage and reuse) is delegated to
    /// <see cref="IAzureClientFactory"/>, which is the single source of truth for the
    /// <c>TokenCredential</c>.
    /// </summary>
    public interface IAzureAccountService
    {
        /// <summary>
        /// Returns <c>true</c> when the user has already signed in during this
        /// application session and a credential is available in the factory.
        /// </summary>
        bool IsSignedIn { get; }

        /// <summary>
        /// Initiates an interactive browser sign-in, stores the resulting credential in
        /// the <see cref="IAzureClientFactory"/> singleton, and returns when complete.
        /// </summary>
        Task SignInAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Enumerates all Azure Storage accounts visible to the signed-in user
        /// across all their subscriptions.  The credential is obtained from the
        /// <see cref="IAzureClientFactory"/> singleton.
        /// </summary>
        Task<IReadOnlyList<StorageAccountInfo>> ListStorageAccountsAsync(
            CancellationToken cancellationToken = default);
    }
}
