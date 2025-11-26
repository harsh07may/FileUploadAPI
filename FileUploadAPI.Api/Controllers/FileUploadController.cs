using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FileUploadAPI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly ILogger<FileUploadController> _logger;
        public FileUploadController(ILogger<FileUploadController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Handle file upload and store into filesystem.
        /// </summary>
        [HttpPost("upload-to-filesystem")]
        public async Task<IActionResult> UploadToFileSystem(IFormFile file)
        {

            return Ok($"File '{file.FileName}' uploaded successfully.");
        }

        /// <summary>
        /// Handle file upload and store into cloud storage.
        /// </summary>
        [HttpPost("upload-to-s3")]
        public async Task<IActionResult> UploadToS3(IFormFile file)
        {
            return Ok($"File '{file.FileName}' uploaded successfully.");
        }

        /// <summary>
        /// Handle file downloads .
        /// </summary>
        [HttpPost("download")]
        public async Task<IActionResult> DownloadFileById(Guid fileId)
        {
            return Ok($"File '{fileId}' downloaded successfully.");
        }

    }
}