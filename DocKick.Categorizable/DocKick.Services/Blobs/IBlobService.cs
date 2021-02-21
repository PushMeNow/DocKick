using System;
using System.IO;
using System.Threading.Tasks;
using DocKick.Dtos.Blobs;

namespace DocKick.Services.Blobs
{
    public interface IBlobService
    {
        Task<BlobUploadModel> Upload(Guid userId, Stream fileStream, string contentType = "application/jpeg");

        Task<BlobDownloadModel> Download(Guid blobId);

        Task<bool> FullDelete(Guid blobId);
    }
}