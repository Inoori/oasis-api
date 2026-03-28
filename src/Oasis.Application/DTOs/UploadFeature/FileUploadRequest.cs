

namespace Oasis.Application.DTOs.UploadFeature;

/// <summary>
/// 文件上传请求 DTO，包含文件键、文件流和内容类型等信息，用于在上传文件时传递相关数据。
/// </summary>
public class FileUploadRequest
{
    public required string Key { get; init; }

    public required Stream FileStream { get; init; }

    public required string ContentType { get; init; }
}