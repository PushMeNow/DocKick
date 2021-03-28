using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using DocKick.Dtos.Blobs;
using DocKick.Entities.Blobs;
using DocKick.Exceptions;
using DocKick.Helpers.Extensions;

namespace DocKick.Services.Blobs
{
    public class BlobService : IBlobService
    {
        private const string BlobContainerName = "dockickcontainer";
        private const int BlobExpirationTime = 8;

        private readonly BlobContainerClient _blobContainer;
        private readonly IBlobDataService _blobDataService;

        public BlobService(BlobServiceClient blobServiceClient, IBlobDataService blobDataService)
        {
            _blobDataService = blobDataService;
            _blobContainer = blobServiceClient.GetBlobContainerClient(BlobContainerName);
        }

        public async Task<BlobUploadModel> Upload(Guid userId, string fileName, Stream fileStream, string contentType = "application/jpeg")
        {
            ExceptionHelper.ThrowArgumentNullIfEmpty(userId, nameof(userId));
            ExceptionHelper.ThrowParameterNullIfEmpty(fileName, nameof(fileName));

            var blobName = GetFullBlobName(userId);
            var blobClient = GetBlobClient(userId, blobName);

            var response = await blobClient.UploadAsync(fileStream,
                                                        new BlobHttpHeaders
                                                        {
                                                            ContentType = contentType
                                                        });

            ExceptionHelper.ThrowNotFoundIfEmpty(response, "Blob");

            var createModel = new BlobModel
                              {
                                  ImageName = fileName,
                                  UserId = userId
                              };

            var createdBlob = await _blobDataService.Create(createModel, blobName);

            var result = new BlobUploadModel
                         {
                             Blob = createdBlob,
                             BlobContentInfo = response.Value
                         };

            return result;
        }

        public Task<BlobModel> GenerateBlobLink(Guid blobId)
        {
            ExceptionHelper.ThrowArgumentNullIfEmpty(blobId, nameof(blobId));

            return _blobDataService.GenerateBlobLink(blobId, GetBlobUrl);
        }

        public async Task<IReadOnlyCollection<BlobModel>> GetBlobsByUserId(Guid userId)
        {
            ExceptionHelper.ThrowArgumentNullIfEmpty(userId, nameof(userId));

            return await _blobDataService.GetBlobsByUserId(userId, GetBlobUrl);
        }

        private BlobClient GetBlobClient(Guid userId, string blobName)
        {
            ExceptionHelper.ThrowArgumentNullIfEmpty(userId, nameof(userId));
            ExceptionHelper.ThrowArgumentNullIfEmpty(blobName, nameof(blobName));

            return _blobContainer.GetBlobClient(blobName);
        }

        private (string url, DateTimeOffset expirationDate) GetBlobUrl(Guid userId, string blobName)
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

        private static string GetFullBlobName(Guid userId)
        {
            return $"{userId}/{Guid.NewGuid()}";
        }

        private static bool IsValidBlobLink(Blob blob)
        {
            return blob.BlobLink is not null && !blob.BlobLink.Url.IsEmpty() && blob.BlobLink.ExpirationDate < DateTimeOffset.Now;
        }
    }
}