using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using DocKick.Data.Repositories;
using DocKick.Entities.Blobs;
using DocKick.Entities.Categories;
using DocKick.Exceptions;

namespace DocKick.Services.Blobs
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IBlobContainerRepository _blobRepository;
        private readonly IRepository<Category> _categoryRepository;

        public BlobService(BlobServiceClient blobServiceClient, IBlobContainerRepository blobRepository, IRepository<Category> categoryRepository)
        {
            _blobServiceClient = blobServiceClient;
            _blobRepository = blobRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<BlobContentInfo> Upload(Guid userId, Guid categoryId, Stream fileStream)
        {
            var containerEntity = await GetContainer(userId);

            ExceptionHelper.ThrowNotFoundIfEmpty(containerEntity, "Container");

            var container = await GetBlobContainer(containerEntity.Name);
            var blobName = $"{Guid.NewGuid()}.jpg";
            var response = await container.UploadBlobAsync(blobName, fileStream);

            ExceptionHelper.ThrowNotFoundIfEmpty(response, "Blob");

            var category = await _categoryRepository.GetById(categoryId);

            var blob = new Blob
                       {
                           Name = blobName,
                           CategoryId = categoryId,
                           BlobContainerId = containerEntity.BlobContainerId
                       };

            category.Blobs.Add(blob);

            await _categoryRepository.Save();

            return response.Value;
        }

        public async Task<BlobDownloadInfo> Download(Guid userId, Guid blobId)
        {
            var containerEntity = await GetContainer(userId);

            ExceptionHelper.ThrowNotFoundIfEmpty(containerEntity, "Container");

            var container = await GetBlobContainer(containerEntity.Name);
            var blob = await _blobRepository.GetById(blobId);
            var client = container.GetBlobClient(blob.Name);
            var response = await client.DownloadAsync();
            
            ExceptionHelper.ThrowNotFoundIfEmpty(response, "Container");

            return response.Value;
        }

        private async Task<BlobContainerClient> GetBlobContainer(string containerName)
        {
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);

            await blobContainerClient.CreateIfNotExistsAsync();

            return blobContainerClient;
        }

        private async Task<BlobContainer> GetContainer(Guid userId)
        {
            var blobContainer = await _blobRepository.GetByUserId(userId);

            if (blobContainer is not null)
            {
                return blobContainer;
            }

            blobContainer = new BlobContainer()
                            {
                                Name = userId.ToString(),
                                UserId = userId
                            };

            await _blobRepository.Create(blobContainer);
            await _blobRepository.Save();

            return blobContainer;
        }
    }
}