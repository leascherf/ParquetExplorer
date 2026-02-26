using Azure.Core;
using Azure.ResourceManager;
using Azure.Storage.Blobs;

namespace ParquetExplorer.Services.Interfaces
{
    /// <summary>
    /// Singleton factory that holds the user's <see cref="TokenCredential"/> and creates
    /// Azure SDK client instances (<see cref="ArmClient"/>, <see cref="BlobServiceClient"/>)
    /// so that every service in the application shares the same credential and therefore the
    /// same underlying MSAL token cache.  This prevents secondary browser prompts when
    /// switching between the Azure Resource Manager (management plane) and Azure Blob
    /// Storage (data plane).
    /// </summary>
    public interface IAzureClientFactory
    {
        /// <summary>
        /// <c>true</c> when a credential has been stored via <see cref="SetCredential"/>.
        /// </summary>
        bool IsSignedIn { get; }

        /// <summary>
        /// Stores a <see cref="TokenCredential"/> that was obtained during the interactive
        /// browser sign-in.  All subsequent calls to <see cref="CreateArmClient"/> and
        /// <see cref="CreateBlobServiceClient"/> will use this credential instance.
        /// </summary>
        void SetCredential(TokenCredential credential);

        /// <summary>
        /// Creates an <see cref="ArmClient"/> using the stored credential.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no credential has been set yet (user has not signed in).
        /// </exception>
        ArmClient CreateArmClient();

        /// <summary>
        /// Creates a <see cref="BlobServiceClient"/> for <paramref name="serviceUri"/>
        /// using the stored credential.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no credential has been set yet (user has not signed in).
        /// </exception>
        BlobServiceClient CreateBlobServiceClient(Uri serviceUri);
    }
}
