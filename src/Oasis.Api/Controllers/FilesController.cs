using Microsoft.AspNetCore.Mvc;
using Oasis.Application.Interfaces;

namespace Oasis.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class FilesController(IFileStorageService storageService) : ControllerBase
{
    private readonly IFileStorageService _fileStorageService = storageService;


    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Upload([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        var stream = file.OpenReadStream();
        var result = await _fileStorageService.UploadFileAsync(stream, file.FileName, file.ContentType);
        return Ok(result);
    }

    [HttpDelete("{fileKey}")]
    public async Task<IActionResult> DeleteFile(string fileKey)
    {
        var result = await _fileStorageService.DeleteFileAsync(fileKey);
        if (result)
            return Ok("File deleted successfully.");
        else
            return NotFound("File not found.");
    }
}