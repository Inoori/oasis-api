using Oasis.Application.DTOs.UploadFeature;

namespace Oasis.Application.Interfaces;

/// <summary>
/// 文件上传请求工厂接口，定义了一个方法用于创建一个新的 FileUploadRequest 实例，包含文件键、文件流和内容类型等信息，用于在上传文件时传递相关数据。
/// </summary>
public interface IFileUploadRequestFactory
{
    FileUploadRequest Build();
}