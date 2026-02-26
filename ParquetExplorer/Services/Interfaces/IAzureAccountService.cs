using Azure.Core;
using ParquetExplorer.Models;

namespace ParquetExplorer.Services.Interfaces
{
    /// <summary>
    /// Abstraction for Azure AD authentication and storage account discovery.
    /// </summary>
    public interface IAzureAccountService
    {
        /// <summary>
        /// Returns <c>true</c> when the user has already signed in during this
        /// application session and a cached credential is available.
        /// </summary>
        bool IsSignedIn { get; }

        /// <summary>
        /// Returns the cached <see cref="TokenCredential"/> from the most recent
        /// successful sign-in, or <c>null</c> if the user has not yet signed in.
        /// </summary>
        TokenCredential? GetCachedCredential();

        /// <summary>
        /// Stores a credential that has been confirmed as working (e.g. after a successful
        /// call to <see cref="ListStorageAccountsAsync"/>). Subsequent calls to
        /// <see cref="IsSignedIn"/> and <see cref="GetCachedCredential"/> will reflect this.
        /// </summary>
        void SetCachedCredential(TokenCredential credential);

        /// <summary>
        /// Initiates an interactive browser sign-in and returns a <see cref="TokenCredential"/>
        /// that can be reused for subsequent Azure SDK calls.
        /// </summary>
        Task<TokenCredential> SignInAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Enumerates all Azure Storage accounts visible to the signed-in user
        /// across all their subscriptions.
        /// </summary>
        Task<IReadOnlyList<StorageAccountInfo>> ListStorageAccountsAsync(
            TokenCredential credential,
            CancellationToken cancellationToken = default);
    }
}
