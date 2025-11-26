using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using FileUploadAPI.Infrastructure.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FileUploadAPI.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<LocalFileStorageOptions>(configuration.GetSection("Storage:Local"));
        services.Configure<S3FileStorageOptions>(configuration.GetSection("Storage:AwsS3"));

        // This can be swapped to anything, and can probably be refactored to a Factory but meh.
        string provider = configuration["Storage:Provider"] ?? "Local"; 

        if (provider.Equals("S3", StringComparison.OrdinalIgnoreCase))
        {

            var accessKey = configuration["Storage:AwsS3:AccessKey"];
            var secretKey = configuration["Storage:AwsS3:SecretKey"];
            var bucket = configuration["Storage:AwsS3:BucketName"];
            var serviceUrl = configuration["Storage:AwsS3:ServiceUrl"];

            var s3Config = new AmazonS3Config
            {
                ServiceURL = serviceUrl,
                ForcePathStyle = true,
            };
            var client = new AmazonS3Client(new BasicAWSCredentials(accessKey, secretKey), s3Config);
            services.AddSingleton<IAmazonS3>(client);
            services.AddSingleton<IFileStorageService, S3StorageService>();
        }
        else
        {
            // Register local file store
            services.AddSingleton<IFileStorageService, LocalFileStorageService>();
        }

        return services;
    }
}
