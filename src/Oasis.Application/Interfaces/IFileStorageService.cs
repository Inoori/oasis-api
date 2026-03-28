namespace Oasis.Application.Interfaces;

public interface IFileStorageService
{
    Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType);

    Task<string> UploadFileWithFactoryAsync(IFileUploadRequestFactory factory);
    
    Task<Stream> DownloadFileAsync(string fileKey);
    Task<bool> DeleteFileAsync(string fileKey);
    Task<string> GetPreSignedUrlAsync(string fileKey, int expirationMinutes = 60);
}