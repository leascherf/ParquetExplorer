using Azure.Core;
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
        public async Task<bool> TrySignInWithEnvironmentVariablesAsync(CancellationToken cancellationToken = default)
        {
            var clientId = Environment.GetEnvironmentVariable("AZURE_CLIENT_ID");
            var clientSecret = Environment.GetEnvironmentVariable("AZURE_CLIENT_SECRET");
            var tenantId = Environment.GetEnvironmentVariable("AZURE_TENANT_ID");

            if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret) || string.IsNullOrEmpty(tenantId))
                return false;

            var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);

            // Eagerly validate the credential so that any errors surface here.
            await credential.GetTokenAsync(
                new TokenRequestContext(new[] { "https://management.azure.com/.default" }),
                cancellationToken).ConfigureAwait(false);

            _clientFactory.SetCredential(credential);
            return true;
        }

        /// <inheritdoc/>
        public async Task SignInAsync(CancellationToken cancellationToken = default)
        {
            var credential = new InteractiveBrowserCredential();

            // Eagerly acquire the management-plane token so that the browser interaction
            // happens exactly here, during the explicit "Sign in" action.
            // After this call the underlying MSAL PublicClientApplication has a cached
            // account + refresh token.  When the data-plane (storage) token is needed
            // later, MSAL uses the cached refresh token silently — no second browser
            // prompt appears.
            await credential.GetTokenAsync(
                new TokenRequestContext(new[] { "https://management.azure.com/.default" }),
                cancellationToken).ConfigureAwait(false);

            _clientFactory.SetCredential(credential);
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyList<StorageAccountInfo>> ListStorageAccountsAsync(
            CancellationToken cancellationToken = default)
        {
            // The ArmClient is created by the factory using the shared credential.
            var armClient = _clientFactory.CreateArmClient();
            var accounts = new List<StorageAccountInfo>();

            await foreach (var subscription in armClient.GetSubscriptions().GetAllAsync(cancellationToken).ConfigureAwait(false))
            {
                string subscriptionName = subscription.Data.DisplayName;

                await foreach (var account in subscription.GetStorageAccountsAsync(cancellationToken: cancellationToken).ConfigureAwait(false))
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
