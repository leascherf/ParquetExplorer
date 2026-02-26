using Azure.Core;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.Storage;
using ParquetExplorer.Models;
using ParquetExplorer.Services.Interfaces;

namespace ParquetExplorer.Services
{
    /// <summary>
    /// Authenticates the user via an interactive browser sign-in and
    /// discovers all Azure Storage accounts across their subscriptions.
    /// The credential is cached in memory for the lifetime of the application.
    /// </summary>
    public class AzureAccountService : IAzureAccountService
    {
        private TokenCredential? _cachedCredential;

        /// <inheritdoc/>
        public bool IsSignedIn => _cachedCredential != null;

        /// <inheritdoc/>
        public TokenCredential? GetCachedCredential() => _cachedCredential;

        /// <inheritdoc/>
        public void SetCachedCredential(TokenCredential credential) =>
            _cachedCredential = credential;

        public Task<TokenCredential> SignInAsync(CancellationToken cancellationToken = default)
        {
            // InteractiveBrowserCredential opens the default browser for OAuth sign-in.
            // The credential is NOT cached here; the caller must call SetCachedCredential
            // after confirming the credential works (e.g. after a successful API call).
            var credential = new InteractiveBrowserCredential();
            return Task.FromResult<TokenCredential>(credential);
        }

        public async Task<IReadOnlyList<StorageAccountInfo>> ListStorageAccountsAsync(
            TokenCredential credential,
            CancellationToken cancellationToken = default)
        {
            var armClient = new ArmClient(credential);
            var accounts = new List<StorageAccountInfo>();

            await foreach (var subscription in armClient.GetSubscriptions().GetAllAsync(cancellationToken))
            {
                string subscriptionName = subscription.Data.DisplayName;

                await foreach (var account in subscription.GetStorageAccountsAsync(cancellationToken: cancellationToken))
                {
                    Uri? blobEndpoint = account.Data.PrimaryEndpoints?.BlobUri;
                    if (blobEndpoint == null) continue;

                    accounts.Add(new StorageAccountInfo
                    {
                        Name = account.Data.Name,
                        SubscriptionName = subscriptionName,
                        BlobEndpoint = blobEndpoint
                    });
                }
            }

            return accounts;
        }
    }
}
