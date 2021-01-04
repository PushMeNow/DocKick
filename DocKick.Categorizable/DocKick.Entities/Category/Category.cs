using System;

namespace DocKick.Entities.Category
{
    public class Category
    {
        public Guid CategoryId { get; set; }
        public Guid? ParentId { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }

        public Category Parent { get; set; }
    }
}