using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Tasty_Treat_be.Interfaces.Service;

namespace Tasty_Treat_be.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<BlobStorageService> _logger;

        public BlobStorageService(
            BlobServiceClient blobServiceClient,
            IConfiguration configuration,
            ILogger<BlobStorageService> logger)
        {
            _blobServiceClient = blobServiceClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<string> UploadImageAsync(IFormFile file, string containerName)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    throw new ArgumentException("File is empty or null");
                }

                // Validate file type
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
                
                if (!allowedExtensions.Contains(fileExtension))
                {
                    throw new ArgumentException($"Invalid file type. Allowed types: {string.Join(", ", allowedExtensions)}");
                }

                // Validate file size (e.g., max 5MB)
                var maxFileSize = 5 * 1024 * 1024; // 5MB
                if (file.Length > maxFileSize)
                {
                    throw new ArgumentException($"File size exceeds maximum allowed size of {maxFileSize / (1024 * 1024)}MB");
                }

                // Get or create container
                var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

                // Generate unique blob name
                var blobName = $"{Guid.NewGuid()}{fileExtension}";
                var blobClient = containerClient.GetBlobClient(blobName);

                // Set content type
                var blobHttpHeaders = new BlobHttpHeaders
                {
                    ContentType = file.ContentType
                };

                // Upload file
                using (var stream = file.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, new BlobUploadOptions
                    {
                        HttpHeaders = blobHttpHeaders
                    });
                }

                _logger.LogInformation($"Successfully uploaded image: {blobName} to container: {containerName}");

                return blobClient.Uri.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error uploading image: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteImageAsync(string imageUrl, string containerName)
        {
            try
            {
                if (string.IsNullOrEmpty(imageUrl))
                {
                    return false;
                }

                // Extract blob name from URL
                var uri = new Uri(imageUrl);
                var blobName = Path.GetFileName(uri.LocalPath);

                var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                var blobClient = containerClient.GetBlobClient(blobName);

                var result = await blobClient.DeleteIfExistsAsync();

                _logger.LogInformation($"Deleted image: {blobName} from container: {containerName}");

                return result.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting image: {ex.Message}");
                return false;
            }
        }

        public async Task<Stream> DownloadImageAsync(string blobName, string containerName)
        {
            try
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                var blobClient = containerClient.GetBlobClient(blobName);

                var response = await blobClient.DownloadAsync();
                return response.Value.Content;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error downloading image: {ex.Message}");
                throw;
            }
        }

        public string GetBlobUrl(string blobName, string containerName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobName);
            return blobClient.Uri.ToString();
        }
    }
}
