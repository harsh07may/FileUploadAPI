using FileUploadAPI.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace FileUploadAPI.Infrastructure.Services;

public class LocalFileStorageService : IFileStorageService
{
    private readonly string _storagePath;

        public LocalFileStorageService(IOptions<LocalFileStorageOptions> options)
        {
            _storagePath = options.Value.Path;

            if (!Directory.Exists(_storagePath))
                Directory.CreateDirectory(_storagePath);
        }

    public async Task<string> SaveFileAsync(Stream fileStream, string fileName, CancellationToken cancellationToken = default)
    {
        var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
        var filePath = Path.Combine(_storagePath, uniqueFileName);

        using var fileStreamDisk = new FileStream(filePath, FileMode.Create);
        await fileStream.CopyToAsync(fileStreamDisk, cancellationToken);

        return uniqueFileName;
    }

    public async Task<Stream> GetFileAsync(string fileName, CancellationToken cancellationToken = default)
    {
        var filePath = Path.Combine(_storagePath, fileName);
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"File {fileName} not found");

        return new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
    }
}
