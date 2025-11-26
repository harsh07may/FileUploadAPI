namespace FileUploadAPI.Infrastructure.Options;

public class LocalFileStorageOptions
{
    public const string SectionName = "Storage:Local";
    public string Path { get; set; } = "./uploads";
}

