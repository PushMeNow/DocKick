using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;

namespace DocKick.Services.Blobs
{
    public interface IBlobService
    {
        Task<BlobContentInfo> Upload(Guid userId,Stream fileStream);
        Task<BlobDownloadInfo> Download(Guid userId, string blobName);
    }
}