using System;

namespace DocKick.Dtos.Blobs
{
    public class BlobModel
    {
        public Guid BlobId { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
    }
}