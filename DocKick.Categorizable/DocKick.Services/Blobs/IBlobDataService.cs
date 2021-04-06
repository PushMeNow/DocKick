using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DocKick.Dtos.Blobs;
using DocKick.Entities.Blobs;

namespace DocKick.Services.Blobs
{
    public interface IBlobDataService
    {
        Task<IReadOnlyCollection<BlobModel>> GetBlobsByUserId(Guid userId, BlobCallback blobCallback);

        Task<BlobModel> Create(BlobModel model, string blobName);

        Task<BlobModel> Update(Guid id, BlobModel model);

        Task<BlobModel> GenerateBlobLink(Guid blobId, BlobCallback blobCallback);

        Task Delete(Guid blobId, Func<Blob, Task> func = null);
    }
}