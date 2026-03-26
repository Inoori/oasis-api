using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Oasis.Application.Interfaces;

namespace Oasis.Infrastructure.Services;


/// <summary>
/// 文件存储服务实现，使用 SeaweedFS 进行文件的上传、下载、删除和预签名 URL 的生成。
/// </summary>
/// <param name="s3Client">Amazon S3 客户端实例</param>
/// <param name="configuration">应用程序配置实例</param>
public class FileStorageService(IAmazonS3 s3Client, IConfiguration configuration) : IFileStorageService
{
    private readonly IAmazonS3 _s3Client = s3Client;
    private readonly string _bucketName = configuration["S3:BucketName"] ?? throw new ArgumentNullException("S3:BucketName", "S3 bucket name is not configured.");


    /// <summary>
    /// 上传文件，将提供的文件流上传到存储服务，并返回一个表示文件位置的字符串（通常是文件键或 URL）。
    /// </summary>
    /// <param name="fileStream">要上传的文件流</param>
    /// <param name="fileName">文件名或键</param>
    /// <param name="contentType">文件的 MIME 类型</param>
    /// <returns>表示文件位置的字符串（通常是文件键或 URL）</returns>
    public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType)
    {
        var putRequest = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = fileName,
            InputStream = fileStream,
            ContentType = contentType
        };

        await _s3Client.PutObjectAsync(putRequest);
        return fileName;
    }

    /// <summary>
    /// 下载文件，根据文件键（fileKey）从存储服务中获取指定的文件，并返回一个包含文件内容的流。
    /// </summary>
    /// <param name="fileKey">文件键</param>
    /// <returns>包含文件内容的流</returns>
    public async Task<Stream> DownloadFileAsync(string fileKey)
    {
        var getRequest = new GetObjectRequest
        {
            BucketName = _bucketName,
            Key = fileKey
        };

        var response = await _s3Client.GetObjectAsync(getRequest);
        return response.ResponseStream;
    }

    /// <summary>
    /// 删除文件，根据文件键（fileKey）从存储服务中删除指定的文件。
    /// </summary>
    /// <param name="fileKey">文件键</param>
    /// <returns>如果文件删除成功，返回 true；否则返回 false。</returns>
    public async Task<bool> DeleteFileAsync(string fileKey)
    {
        var deleteRequest = new DeleteObjectRequest
        {
            BucketName = _bucketName,
            Key = fileKey
        };

        await _s3Client.DeleteObjectAsync(deleteRequest);
        return true;
    }


    /// <summary>
    /// 生成预签名 URL，允许客户端在指定时间内访问文件，而无需直接暴露存储服务的凭据。
    /// </summary>
    /// <param name="fileKey">文件键</param>
    /// <param name="expirationMinutes">URL 的有效期（分钟）</param>
    /// <returns>预签名 URL 字符串</returns>
    public async Task<string> GetPreSignedUrlAsync(string fileKey, int expirationMinutes = 60)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = _bucketName,
            Key = fileKey,
            Expires = DateTime.UtcNow.AddMinutes(expirationMinutes)
        };
        return await Task.FromResult(_s3Client.GetPreSignedURL(request));
    }




//todo
    public async Task UploadImageAsync(Stream fileStream, string fileName)
    {

        var provider = new FileExtensionContentTypeProvider();
        if (!provider.TryGetContentType(fileName, out var contentType))
        {
            contentType = "application/octet-stream";
        }




        var putRequest = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = fileName,
            InputStream = fileStream,
            ContentType = contentType
        };

        await _s3Client.PutObjectAsync(putRequest);
    }
}