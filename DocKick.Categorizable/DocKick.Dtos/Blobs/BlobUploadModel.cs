using Azure.Storage.Blobs.Models;

namespace DocKick.Dtos.Blobs
{
    public class BlobUploadModel : BlobModel
    {
        public BlobContentInfo BlobContentInfo { get; set; }
    }
}