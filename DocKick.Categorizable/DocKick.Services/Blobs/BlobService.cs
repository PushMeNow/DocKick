using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using DocKick.Data.Repositories;
using DocKick.Dtos.Blobs;
using DocKick.Entities.Blobs;
using DocKick.Exceptions;
using DocKick.Helpers.Extensions;

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

            var createModel = new BlobModel
                              {
                                  Name = blobName,
                                  UserId = userId
                              };

            var model = new BlobUploadModel
                        {
                            Blob = await Create(createModel),
                            BlobContentInfo = response.Value
                        };

            return model;
        }

        [Obsolete("Planning to use GenerateBlobLink")]
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
                            Name = blob.Name,
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

        public async Task<BlobLinkModel> GenerateBlobLink(Guid blobId)
        {
            var blob = await Repository.GetById(blobId);

            if (blob.BlobLink is not null
                && !blob.BlobLink.Url.IsEmpty()
                && blob.BlobLink.ExpirationDate < DateTimeOffset.Now)
            {
                return new BlobLinkModel
                       {
                           Blob = Map<BlobModel>(blob),
                           Url = blob.BlobLink.Url,
                           ExpirationDate = blob.BlobLink.ExpirationDate
                       };
            }

            ExceptionHelper.ThrowNotFoundIfEmpty(blob, "Blob");

            var (blobUrl, expirationDate) = GetBlobUrl(blob.UserId, blob.Name);

            blob.BlobLink ??= new BlobLink();
            blob.BlobLink.ExpirationDate = expirationDate;
            blob.BlobLink.Url = blobUrl;

            Repository.Update(blob);
            await Repository.Save();

            var result = new BlobLinkModel
                         {
                             Blob = Map<BlobModel>(blob),
                             Url = blob.BlobLink.Url,
                             ExpirationDate = blob.BlobLink.ExpirationDate
                         };

            return result;
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

        private (string uri, DateTimeOffset expirationDate) GetBlobUrl(Guid userId, string blobName)
        {
            var client = GetBlobClient(userId, blobName);
            var expirationDate = DateTimeOffset.Now.AddHours(1);
            var builder = new BlobSasBuilder(BlobSasPermissions.Read, expirationDate)
                          {
                              BlobContainerName = client.GetParentBlobContainerClient()
                                                        .Name,
                              Resource = "b",
                              BlobName = client.Name
                          };

            var blobUri = client.GenerateSasUri(builder);

            return (blobUri.ToString(), expirationDate);
        }
    }
}