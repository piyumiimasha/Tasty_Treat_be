namespace Tasty_Treat_be.Interfaces.Service
{
    public interface IBlobStorageService
    {
        Task<string> UploadImageAsync(IFormFile file, string containerName);
        Task<bool> DeleteImageAsync(string imageUrl, string containerName);
        Task<Stream> DownloadImageAsync(string blobName, string containerName);
        string GetBlobUrl(string blobName, string containerName);
    }
}
