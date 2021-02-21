using System;
using System.Threading.Tasks;
using DocKick.Dtos.Categories;

namespace DocKick.Services.Categories
{
    public interface ICategoryService : IDataService<CategoryModel, Guid>
    {
        Task<CategoryModel[]> GetCategoriesByUserId(Guid userId);

        Task<CategoryModel[]> GetCategoryTreeByUserId(Guid userId);
    }
}