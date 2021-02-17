using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using DocKick.Data.Repositories;
using DocKick.Dtos.Blobs;
using DocKick.Entities.Blobs;
using DocKick.Entities.Categories;
using DocKick.Exceptions;

namespace DocKick.Services.Blobs
{
    public class BlobService : IBlobService
    {
        private const string BlobContainerName = "dockickcontainer";

        private readonly IRepository<Blob> _blobRepository;
        private readonly BlobServiceClient _blobServiceClient;

        public BlobService(BlobServiceClient blobServiceClient, IRepository<Blob> blobRepository)
        {
            _blobServiceClient = blobServiceClient;
            _blobRepository = blobRepository;
        }

        public async Task<BlobUploadModel> Upload(Guid userId, Stream fileStream, string contentType = "application/jpeg")
        {
            ExceptionHelper.ThrowArgumentNullIfEmpty(userId, nameof(userId));
            
            var container = _blobServiceClient.GetBlobContainerClient(BlobContainerName);
            var blobName = $"{Guid.NewGuid()}.jpg";
            var blobClient = container.GetBlobClient(GetFullBlobName(userId, blobName));

            var response = await blobClient.UploadAsync(fileStream,
                                                        new BlobHttpHeaders
                                                        {
                                                            ContentType = contentType
                                                        });

            ExceptionHelper.ThrowNotFoundIfEmpty(response, "Blob");

            var blob = new Blob
                       {
                           Name = blobName,
                           UserId = userId
                       };

            await _blobRepository.Create(blob);
            await _blobRepository.Save();

            var model = new BlobUploadModel
                        {
                            BlobId = blob.BlobId,
                            BlobName = blob.Name,
                            UserId = userId,
                            BlobContentInfo = response.Value
                        };

            return model;
        }

        public async Task<BlobDownloadModel> Download(Guid blobId)
        {
            var blob = await _blobRepository.GetById(blobId);

            ExceptionHelper.ThrowNotFoundIfEmpty(blob, "Blob");

            var container = _blobServiceClient.GetBlobContainerClient(BlobContainerName);
            var client = container.GetBlobClient(GetFullBlobName(blob.Category.UserId, blob.Name));
            var response = await client.DownloadAsync();

            ExceptionHelper.ThrowNotFoundIfEmpty(response, "Container");

            var model = new BlobDownloadModel
                        {
                            BlobId = blob.BlobId,
                            BlobName = blob.Name,
                            CategoryId = blob.CategoryId,
                            BlobDownloadInfo = response.Value
                        };

            return model;
        }

        private static string GetFullBlobName(Guid userId, string blobName)
        {
            return $"{userId}/{blobName}";
        }
    }
}