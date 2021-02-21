using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using DocKick.Data.Repositories;
using DocKick.Dtos.Blobs;
using DocKick.Dtos.Categories;
using DocKick.Entities.Blobs;
using DocKick.Entities.Categories;
using DocKick.Exceptions;

namespace DocKick.Services.Blobs
{
    public class BlobService : BaseDataService<IRepository<Blob>, Blob, BlobModel, Guid>, IBlobService
    {
        private const string BlobContainerName = "dockickcontainer";

        private readonly BlobContainerClient _blobContainer;

        public BlobService(BlobServiceClient blobServiceClient, IRepository<Blob> blobRepository, IMapper mapper) : base(blobRepository, mapper)
        {
            _blobContainer = blobServiceClient.GetBlobContainerClient(BlobContainerName);
        }

        public async Task<BlobUploadModel> Upload(Guid userId, Stream fileStream, string contentType = "application/jpeg")
        {
            ExceptionHelper.ThrowArgumentNullIfEmpty(userId, nameof(userId));

            var blobName = $"{Guid.NewGuid()}.jpg";
            var blobClient = GetBlobClient(userId, blobName);

            var response = await blobClient.UploadAsync(fileStream,
                                                        new BlobHttpHeaders
                                                        {
                                                            ContentType = contentType
                                                        });

            ExceptionHelper.ThrowNotFoundIfEmpty(response, "Blob");

            var createModel = new BlobModel()
                              {
                                  BlobName = blobName,
                                  UserId = userId
                              };

            var model = new BlobUploadModel
                        {
                            Blob = await Create(createModel),
                            BlobContentInfo = response.Value
                        };

            return model;
        }

        // TODO: Need to figure out how to init blob link in Azure Storage.
        public async Task<BlobDownloadModel> Download(Guid blobId)
        {
            var blob = await Repository.GetById(blobId);

            ExceptionHelper.ThrowNotFoundIfEmpty(blob, "Blob");

            var client = GetBlobClient(blob.UserId, blob.Name);
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

        public async Task<bool> FullDelete(Guid blobId)
        {
            ExceptionHelper.ThrowArgumentNullIfEmpty(blobId, nameof(blobId));

            var blobEntity = await Repository.GetById(blobId);

            if (blobEntity is null)
            {
                return true;
            }

            var client = GetBlobClient(blobEntity.UserId, blobEntity.Name);
            var response = await client.DeleteIfExistsAsync();

            ExceptionHelper.ThrowParameterNullIfEmpty(response, "Incorrect response of Azure Blobs.");

            await Delete(blobId);

            return response.Value;
        }

        private static string GetFullBlobName(Guid userId, string blobName)
        {
            return $"{userId}/{blobName}";
        }

        private BlobClient GetBlobClient(Guid userId, string blobName)
        {
            ExceptionHelper.ThrowArgumentNullIfEmpty((userId, nameof(userId)), (blobName, nameof(blobName)));

            return _blobContainer.GetBlobClient(GetFullBlobName(userId, blobName));
        }
    }
}