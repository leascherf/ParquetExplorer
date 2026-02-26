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
    /// </summary>
    public class AzureAccountService : IAzureAccountService
    {
        public Task<TokenCredential> SignInAsync(CancellationToken cancellationToken = default)
        {
            // InteractiveBrowserCredential opens the default browser for OAuth sign-in.
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
