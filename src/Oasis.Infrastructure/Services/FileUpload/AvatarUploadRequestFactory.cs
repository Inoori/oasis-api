using Microsoft.AspNetCore.StaticFiles;
using Oasis.Application.DTOs.UploadFeature;
using Oasis.Application.Interfaces;

namespace Oasis.Infrastructure.Services.FileUpload;

/// <summary>
/// 头像上传 文件请求工厂实现类
/// </summary>
/// <param name="fileName">文件名</param>
/// <param name="fileStream">文件流</param>
/// <param name="userId">用户ID</param>
/// <param name="contentType">内容类型</param>
public class AvatarUploadRequestFactory(string fileName, Stream fileStream, string userId, string? contentType = null) : IFileUploadRequestFactory
{
    public FileUploadRequest Build()
    {
        string extension = Path.GetExtension(fileName).ToLowerInvariant();

        if (string.IsNullOrEmpty(contentType) && !new FileExtensionContentTypeProvider().TryGetContentType(fileName, out contentType))
        {
            contentType = "application/octet-stream"; // 默认内容类型
        }

        return new FileUploadRequest
        {
            Key = $"avatars/{userId}/avatar{extension}", // 使用用户 ID 作为文件夹名称，确保每个用户的头像存储在不同的路径下，并固定文件名为avatar，每次上传自动覆盖之前的头像
            FileStream = fileStream,
            ContentType = contentType
        };
    }
}