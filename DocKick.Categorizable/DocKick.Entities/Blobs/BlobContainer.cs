using System;
using System.Collections.Generic;

namespace DocKick.Entities.Blobs
{
    public class BlobContainer
    {
        public Guid BlobContainerId { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Blob> Blobs { get; set; }
    }
}