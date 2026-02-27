using ParquetExplorer.Models;

namespace ParquetExplorer.Services.Interfaces
{
    /// <summary>
    /// Abstraction for SFTP operations used by the file-picker and compare features.
    /// Decouples consumers from the concrete SSH.NET implementation.
    /// </summary>
    public interface ISftpService
    {
        /// <summary>
        /// Lists the immediate children (files and sub-directories) of <paramref name="path"/>
        /// on the remote server.  Entries named "." and ".." are excluded.
        /// Directories appear before files; within each group entries are sorted by name.
        /// </summary>
        Task<IReadOnlyList<SftpFileEntry>> ListDirectoryAsync(
            string host, int port, string username, string password, string path,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Downloads the remote file at <paramref name="remotePath"/> to a local temp file
        /// and returns that temp-file path.  The caller is responsible for deleting the file
        /// when it is no longer needed.
        /// </summary>
        Task<string> DownloadFileToTempAsync(
            string host, int port, string username, string password, string remotePath,
            CancellationToken cancellationToken = default);
    }
}
