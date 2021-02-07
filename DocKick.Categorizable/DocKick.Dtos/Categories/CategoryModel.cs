using System;

namespace DocKick.Dtos.Categories
{
    public class CategoryModel
    {
        public Guid CategoryId { get; set; }
        public Guid ParentId { get; set; }
        public string Name { get; set; }
    }
}