using ParquetExplorer.Models;
using ParquetExplorer.Services.Interfaces;
using Renci.SshNet;

namespace ParquetExplorer.Services
{
    /// <summary>
    /// SFTP implementation of <see cref="ISftpService"/> using the SSH.NET library.
    /// Each call opens a fresh connection, performs the operation, and disconnects,
    /// so callers do not need to manage connection lifetimes.
    /// </summary>
    public class SftpService : ISftpService
    {
        /// <inheritdoc/>
        public Task<IReadOnlyList<SftpFileEntry>> ListDirectoryAsync(
            string host, int port, string username, string password, string path,
            CancellationToken cancellationToken = default)
        {
            return Task.Run(() =>
            {
                using var client = new SftpClient(host, port, username, password);
                client.Connect();
                try
                {
                    var entries = client.ListDirectory(path)
                        .Where(e => e.Name != "." && e.Name != "..")
                        .Select(e => new SftpFileEntry
                        {
                            Name      = e.Name,
                            FullPath  = e.FullName,
                            IsDirectory = e.IsDirectory,
                        })
                        .OrderBy(e => !e.IsDirectory)
                        .ThenBy(e => e.Name, StringComparer.OrdinalIgnoreCase)
                        .ToList();

                    return (IReadOnlyList<SftpFileEntry>)entries;
                }
                finally
                {
                    client.Disconnect();
                }
            }, cancellationToken);
        }

        /// <inheritdoc/>
        public Task<string> DownloadFileToTempAsync(
            string host, int port, string username, string password, string remotePath,
            CancellationToken cancellationToken = default)
        {
            return Task.Run(() =>
            {
                using var client = new SftpClient(host, port, username, password);
                client.Connect();
                try
                {
                    string extension = Path.GetExtension(remotePath);
                    string tempFile = Path.Combine(
                        Path.GetTempPath(),
                        $"parquetexplorer_{Guid.NewGuid()}{extension}");

                    using var stream = File.Create(tempFile);
                    client.DownloadFile(remotePath, stream);
                    return tempFile;
                }
                finally
                {
                    client.Disconnect();
                }
            }, cancellationToken);
        }
    }
}
