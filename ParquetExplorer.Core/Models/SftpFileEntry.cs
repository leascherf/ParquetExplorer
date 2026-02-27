namespace ParquetExplorer.Models
{
    /// <summary>
    /// Represents a single file or directory entry returned by
    /// <see cref="Services.Interfaces.ISftpService.ListDirectoryAsync"/>.
    /// </summary>
    public class SftpFileEntry
    {
        /// <summary>Short name of the entry (no path components).</summary>
        public string Name { get; init; } = string.Empty;

        /// <summary>Full absolute path on the remote server.</summary>
        public string FullPath { get; init; } = string.Empty;

        /// <summary>True when this entry represents a directory.</summary>
        public bool IsDirectory { get; init; }
    }
}
