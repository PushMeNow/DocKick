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
        private readonly IRepository<Category> _categoryRepository;

        public BlobService(BlobServiceClient blobServiceClient, IRepository<Blob> blobRepository, IRepository<Category> categoryRepository)
        {
            _blobServiceClient = blobServiceClient;
            _blobRepository = blobRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<BlobUploadModel> Upload(Guid categoryId, Stream fileStream, string contentType = "application/jpeg")
        {
            var category = await _categoryRepository.GetById(categoryId);

            ExceptionHelper.ThrowNotFoundIfEmpty(category, "Category");

            var container = _blobServiceClient.GetBlobContainerClient(BlobContainerName);
            var blobName = $"{Guid.NewGuid()}.jpg";
            var blobClient = container.GetBlobClient(GetFullBlobName(category.UserId, blobName));

            var response = await blobClient.UploadAsync(fileStream,
                                                        new BlobHttpHeaders
                                                        {
                                                            ContentType = contentType
                                                        });

            ExceptionHelper.ThrowNotFoundIfEmpty(response, "Blob");

            var blob = new Blob
                       {
                           Name = blobName,
                           CategoryId = categoryId
                       };

            category.Blobs.Add(blob);

            await _categoryRepository.Save();

            var model = new BlobUploadModel
                        {
                            BlobId = blob.BlobId,
                            CategoryId = categoryId,
                            BlobName = blob.Name,
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