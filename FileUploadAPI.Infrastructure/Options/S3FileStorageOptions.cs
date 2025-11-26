using System;
using System.Collections.Generic;
using System.Text;

namespace FileUploadAPI.Infrastructure.Options
{
    public class S3FileStorageOptions
    {
        public const string SectionName = "Storage:AwsS3";
        public string BucketName { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty; // Optional if configured globally
    }
}
