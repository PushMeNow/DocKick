using System;

namespace DocKick.Dtos.Categories
{
    public class CategoryRequest : CreateCategoryRequest
    {
        public Guid CategoryId { get; set; }
    }
}