using Azure.Storage.Blobs.Models;

namespace DocKick.Dtos.Blobs
{
    public class BlobDownloadModel : BlobModel
    {
        public BlobDownloadInfo BlobDownloadInfo { get; set; }
    }
}