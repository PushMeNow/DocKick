using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DocKick.Dtos.Blobs;

namespace DocKick.Services.Blobs
{
    public interface IBlobDataService
    {
        Task<IReadOnlyCollection<BlobModel>> GetBlobsByUserId(Guid userId, BlobCallback blobCallback);

        Task<BlobModel> Create(BlobModel model, string blobName);

        Task<BlobModel> Update(Guid id, BlobModel model);

        Task Delete(Guid blobId);

        Task<BlobModel> GenerateBlobLink(Guid blobId, BlobCallback blobCallback);
    }
}