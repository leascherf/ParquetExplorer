namespace ParquetExplorer.Models
{
    /// <summary>
    /// Represents an Azure Storage account discovered via Azure Resource Manager.
    /// </summary>
    public class StorageAccountInfo
    {
        /// <summary>Storage account name (e.g. "mystorageaccount").</summary>
        public string Name { get; init; } = string.Empty;

        /// <summary>The Azure subscription that owns this account.</summary>
        public string SubscriptionName { get; init; } = string.Empty;

        /// <summary>Blob service endpoint URI (e.g. https://mystorageaccount.blob.core.windows.net/).</summary>
        public Uri BlobEndpoint { get; init; } = null!;

        public override string ToString() => $"{Name}  [{SubscriptionName}]";
    }
}
