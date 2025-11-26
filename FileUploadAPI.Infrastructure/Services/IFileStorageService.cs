namespace FileUploadAPI.Infrastructure.Services;

public interface IFileStorageService
{
    /// <summary>
    /// Uploads a file to the configured storage provider.
    /// </summary>
    /// <returns>The unique identifier or path of the stored file.</returns>
    Task<string> SaveFileAsync(Stream fileStream, string fileName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a file stream.
    /// </summary>
    Task<Stream> GetFileAsync(string fileName, CancellationToken cancellationToken = default);
}
