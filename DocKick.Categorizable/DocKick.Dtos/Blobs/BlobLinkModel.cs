using System;

namespace DocKick.Dtos.Blobs
{
    public class BlobLinkModel
    {
        public Guid BlobLinkId { get; set; }
        public string Url { get; set; }
        public DateTimeOffset ExpirationDate { get; set; }
    }
}