using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DocKick.Dtos.Blobs;

namespace DocKick.Services.Blobs
{
    public interface IBlobService
    {
        Task<BlobUploadModel> Upload(Guid userId, string fileName, Stream fileStream, string contentType = "application/jpeg");

        Task<BlobModel> GenerateBlobLink(Guid blobId);

        Task<IReadOnlyCollection<BlobModel>> GetBlobsByUserId(Guid userId);
    }
}