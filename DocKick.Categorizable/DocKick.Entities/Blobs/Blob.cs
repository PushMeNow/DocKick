using System;
using DocKick.Entities.Categories;

namespace DocKick.Entities.Blobs
{
    public class Blob
    {
        public Guid BlobId { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }

        public virtual Category Category { get; set; }
        public virtual BlobLink BlobLink { get; set; }
    }
}