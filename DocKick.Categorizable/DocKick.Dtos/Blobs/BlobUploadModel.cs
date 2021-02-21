using Azure.Storage.Blobs.Models;

namespace DocKick.Dtos.Blobs
{
    public class BlobUploadModel
    {
        public BlobModel Blob { get; set; }
        public BlobContentInfo BlobContentInfo { get; set; }
    }
}