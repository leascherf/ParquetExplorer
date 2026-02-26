using Azure.Core;
using Azure.ResourceManager;
using Azure.Storage.Blobs;
using ParquetExplorer.Services.Interfaces;

namespace ParquetExplorer.Services
{
    /// <summary>
    /// Singleton factory that is the single source of truth for the user's
    /// <see cref="TokenCredential"/>.  Because every Azure SDK client
    /// (<see cref="ArmClient"/>, <see cref="BlobServiceClient"/>, …) is created from
    /// the same credential instance, MSAL's internal token cache is shared across all
    /// callers.  After the user authenticates once (management-plane scope), the same
    /// MSAL session silently acquires the storage-plane token without opening another
    /// browser window.
    /// <para>
    /// <b>Thread safety:</b> This class is designed for WinForms applications where all
    /// UI interactions (and therefore all calls to <see cref="SetCredential"/> and the
    /// <c>Create*</c> methods) run on the single UI thread.  No additional
    /// synchronisation is applied.
    /// </para>
    /// </summary>
    public class AzureClientFactory : IAzureClientFactory
    {
        private TokenCredential? _credential;

        /// <inheritdoc/>
        public bool IsSignedIn => _credential != null;

        /// <inheritdoc/>
        public void SetCredential(TokenCredential credential) => _credential = credential;

        /// <inheritdoc/>
        public ArmClient CreateArmClient()
        {
            if (_credential == null)
                throw new InvalidOperationException("Not signed in. Call SetCredential before creating clients.");
            return new ArmClient(_credential);
        }

        /// <inheritdoc/>
        public BlobServiceClient CreateBlobServiceClient(Uri serviceUri)
        {
            if (_credential == null)
                throw new InvalidOperationException("Not signed in. Call SetCredential before creating clients.");
            return new BlobServiceClient(serviceUri, _credential);
        }
    }
}
