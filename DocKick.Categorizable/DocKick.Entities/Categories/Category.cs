using System;
using System.Collections.Generic;
using DocKick.Entities.Blobs;

namespace DocKick.Entities.Categories
{
    public class Category
    {
        public Guid CategoryId { get; set; }
        public Guid? ParentId { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }

        public virtual Category Parent { get; set; }
        public virtual ICollection<Category> Children { get; set; }
        public virtual ICollection<Blob> Blobs { get; set; }
    }
}