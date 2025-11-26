using Amazon.S3.Model;
using FileUploadAPI.Infrastructure.Options;

namespace FileUploadAPI.Infrastructure.Services;

public class S3StorageService : IFileStorageService
{

    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;

    public S3StorageService(IAmazonS3 s3Client, IOptions<S3FileStorageOptions> options)
    {
        _s3Client = s3Client;
        _bucketName =  options.Value.BucketName ?? throw new ArgumentException("S3 bucket name not configured");
    }

    public async Task<string> SaveFileAsync(Stream fileStream, string fileName, CancellationToken cancellationToken = default)
    {
        var key = $"{Guid.NewGuid()}_{fileName}";
        
        var request = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = key,
            InputStream = fileStream,

        };

        await _s3Client.PutObjectAsync(request, cancellationToken);

        return key;
    }

    public async Task<Stream> GetFileAsync(string key, CancellationToken cancellationToken = default)
    {

        var request = new GetObjectRequest
        {
            BucketName = _bucketName,
            Key = key
        };

        var response = await _s3Client.GetObjectAsync(request, cancellationToken);
        return response.ResponseStream;

    }

}
