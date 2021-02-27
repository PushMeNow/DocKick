using System;

namespace DocKick.Entities.Blobs
{
    public class BlobLink
    {
        public Guid BlobLinkId { get; set; }
        public Guid BlobId { get; set; }
        public string Url { get; set; }
        public DateTimeOffset ExpirationDate { get; set; }

        public virtual Blob Blob { get; set; }
    }
}