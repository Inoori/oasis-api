using Microsoft.AspNetCore.Http;

namespace Oasis.Application.DTOs.UploadFeature;


public class UploadAvatarRequest
{
    public required string UserId { get; set; }
    public required IFormFile File { get; set; }
}