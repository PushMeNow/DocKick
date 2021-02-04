using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace DocKick.Services.Blobs
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public BlobService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task<BlobContentInfo> Upload(Guid userId, Stream fileStream)
        {
            var container = await GetBlobContainer(userId);
            var blobName = $"{Guid.NewGuid()}.jpg";
            var response = await container.UploadBlobAsync(blobName, fileStream);
            
            return response?.Value;
        }

        public async Task<BlobDownloadInfo> Download(Guid userId, string blobName)
        {
            var container = await GetBlobContainer(userId);
            var client = container.GetBlobClient(blobName);
            var response = await client.DownloadAsync();

            return response?.Value;
        }

        private async Task<BlobContainerClient> GetBlobContainer(Guid userId)
        {
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(userId.ToString());

            await blobContainerClient.CreateIfNotExistsAsync();

            return blobContainerClient;
        }
    }
}