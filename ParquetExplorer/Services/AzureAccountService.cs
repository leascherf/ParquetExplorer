using Azure.Identity;
using Azure.ResourceManager.Storage;
using ParquetExplorer.Models;
using ParquetExplorer.Services.Interfaces;

namespace ParquetExplorer.Services
{
    /// <summary>
    /// Authenticates the user via an interactive browser sign-in and
    /// discovers all Azure Storage accounts across their subscriptions.
    /// The <see cref="TokenCredential"/> is stored in the <see cref="IAzureClientFactory"/>
    /// singleton so that both the management plane (<c>ArmClient</c>) and the data plane
    /// (<c>BlobServiceClient</c>) share the same MSAL token cache and the user is never
    /// prompted to log in twice.
    /// </summary>
    public class AzureAccountService : IAzureAccountService
    {
        private readonly IAzureClientFactory _clientFactory;

        public AzureAccountService(IAzureClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        /// <inheritdoc/>
        public bool IsSignedIn => _clientFactory.IsSignedIn;

        /// <inheritdoc/>
        public Task SignInAsync(CancellationToken cancellationToken = default)
        {
            // InteractiveBrowserCredential opens the default browser for OAuth sign-in.
            // The browser prompt is deferred until the first GetTokenAsync call (lazy).
            // MSAL serialises concurrent token requests internally, so if multiple
            // Azure SDK operations are initiated before the first token is acquired they
            // will all queue behind the same browser interaction rather than opening
            // multiple windows.
            //
            // Storing the credential in the factory ensures that every Azure SDK client
            // created afterwards (ArmClient, BlobServiceClient, …) shares the same MSAL
            // session and can silently acquire tokens for additional scopes without
            // prompting the user again.
            var credential = new InteractiveBrowserCredential();
            _clientFactory.SetCredential(credential);
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyList<StorageAccountInfo>> ListStorageAccountsAsync(
            CancellationToken cancellationToken = default)
        {
            // The ArmClient is created by the factory using the shared credential.
            var armClient = _clientFactory.CreateArmClient();
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
