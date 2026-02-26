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
