using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using FileUploadAPI.Infrastructure.Options;

namespace FileUploadAPI.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Bind options
        services.Configure<LocalFileStorageOptions>(configuration.GetSection("Storage:Local"));
        services.Configure<S3FileStorageOptions>(configuration.GetSection("Storage:AwsS3"));

        string provider = configuration["Storage:Provider"] ?? "Local";
        if (provider.Equals("S3", StringComparison.OrdinalIgnoreCase))
        {
            // Register AWS SDK & Implementation
            services.AddAWSService<IAmazonS3>(new AWSOptions
            {
                Credentials = new BasicAWSCredentials(configuration["Storage:AwsS3:AccessKey"], configuration["Storage:AwsS3:AccessKey"])
            });
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
