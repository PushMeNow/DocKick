using System;

namespace DocKick.Dtos.Categories
{
    public class CategoryRequest
    {
        public Guid UserId { get; set; }
        public Guid? ParentId { get; set; }
        public string Name { get; set; }
    }
}