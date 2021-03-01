using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using Microsoft.EntityFrameworkCore;

namespace DocKick.Services.Blobs
{
    public class BlobService : BaseDataService<IRepository<Blob>, Blob, BlobModel, Guid>, IBlobService
    {
        private const string BlobContainerName = "dockickcontainer";
        private const int BlobExpirationTime = 8;

        private readonly BlobContainerClient _blobContainer;

        public BlobService(BlobServiceClient blobServiceClient, IRepository<Blob> blobRepository, IMapper mapper) : base(blobRepository, mapper)
        {
            _blobContainer = blobServiceClient.GetBlobContainerClient(BlobContainerName);
        }

        public async Task<IReadOnlyCollection<BlobModel>> GetBlobsByUserId(Guid userId)
        {
            ExceptionHelper.ThrowArgumentNullIfEmpty(userId, nameof(userId));

            var blobs = await Repository.GetAll()
                                        .Include(q => q.BlobLink)
                                        .Where(q => q.UserId == userId)
                                        .ToArrayAsync();

            foreach (var blob in blobs.Where(blob => !IsValidBlobLink(blob)))
            {
                await GenerateBlobLinkAndSave(blob);
            }

            return Map<BlobModel[]>(blobs);
        }

        public async Task<BlobUploadModel> Upload(Guid userId, string fileName, Stream fileStream, string contentType = "application/jpeg")
        {
            ExceptionHelper.ThrowArgumentNullIfEmpty(userId, nameof(userId));

            var blobName = fileName.IsEmpty() ? $"{Guid.NewGuid()}.jpg" : fileName;

            await CheckFileName(blobName, userId);

            var blobClient = GetBlobClient(userId, blobName);

            await CheckBlob(blobClient, blobName, userId);

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

            var createdBlob = await Create(createModel);

            var result = new BlobUploadModel
                         {
                             Blob = createdBlob,
                             BlobContentInfo = response.Value
                         };

            return result;
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

        public override async Task<bool> Delete(Guid blobId)
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

            await base.Delete(blobId);

            return response.Value;
        }

        public async Task<BlobModel> GenerateBlobLink(Guid blobId)
        {
            ExceptionHelper.ThrowArgumentNullIfEmpty(blobId, nameof(blobId));

            var blob = await Repository.GetById(blobId);

            ExceptionHelper.ThrowNotFoundIfEmpty(blob, "Blob");

            if (IsValidBlobLink(blob))
            {
                return Map<BlobModel>(blob);
            }

            blob = await GenerateBlobLinkAndSave(blob);

            return Map<BlobModel>(blob);
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
            var expirationDate = DateTimeOffset.Now.AddHours(BlobExpirationTime);
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

        private async Task<Blob> GenerateBlobLinkAndSave(Blob blob)
        {
            var (blobUrl, expirationDate) = GetBlobUrl(blob.UserId, blob.Name);

            blob.BlobLink ??= new BlobLink();
            blob.BlobLink.ExpirationDate = expirationDate;
            blob.BlobLink.Url = blobUrl;

            await Repository.Save();

            return blob;
        }

        private static bool IsValidBlobLink(Blob blob)
        {
            return blob.BlobLink is not null && !blob.BlobLink.Url.IsEmpty() && blob.BlobLink.ExpirationDate < DateTimeOffset.Now;
        }

        private async Task CheckFileName(string fileName, Guid userId)
        {
            var isValidFileName = await Repository.GetAll()
                                                  .AnyAsync(q => q.Name == fileName && q.UserId == userId);

            ExceptionHelper.ThrowParameterInvalidIfTrue(isValidFileName, "The user already has the same file.");
        }

        private async Task CheckBlob(BlobBaseClient blobClient, string blobName, Guid userId)
        {
            if (await blobClient.ExistsAsync())
            {
                // if system come here it means we have blob in azure but not in DB
                // so need to create record about thi blob in DB and throw exception.
                await Create(new BlobModel
                             {
                                 Name = blobName,
                                 UserId = userId
                             });

                throw new ParameterInvalidException("The user already has the same file.");
            }
        }
    }
}