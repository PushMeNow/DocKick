﻿using System;
using System.Threading.Tasks;
using DocKick.Dtos.Categories;

namespace DocKick.Services.Categories
{
    public interface ICategoryService : IDataService<CategoryModel, CategoryRequest, Guid>
    {
        Task<CategoryModel[]> GetCategoriesByUserId(Guid userId);

        Task<CategoryModel> UpdateParent(Guid categoryId, Guid parentId);
    }
}