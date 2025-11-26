using FileUploadAPI.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FileUploadAPI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly IFileStorageService _storageService;
        private readonly ILogger<FileUploadController> _logger;
        public FileUploadController(IFileStorageService storageService, ILogger<FileUploadController> logger)
        {
            _storageService = storageService;
            _logger = logger;
        }

        /// <summary>
        /// Handle file upload and store into filesystem.
        /// </summary>
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            using var stream = file.OpenReadStream();
            var fileId = await _storageService.SaveFileAsync(stream, file.FileName);

            return Ok(new { 
                Message = "File uploaded successfully", 
                FileId = fileId 
            });
        }

        /// <summary>
        /// Handle file downloads .
        /// </summary>
        [HttpPost("download")]
        public async Task<IActionResult> Download(string fileName)
        {
            try
            {
                var stream = await _storageService.GetFileAsync(fileName);
                return File(stream, "application/octet-stream", fileName);
            }
            catch (FileNotFoundException)
            {
                return NotFound();
            }
        }

    }
}