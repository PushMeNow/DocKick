using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DocKick.Data.Repositories;
using DocKick.Dtos.Categories;
using DocKick.Entities.Categories;
using DocKick.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DocKick.Services.Categories
{
    public class CategoryService : BaseDataService<IRepository<Category>, Category, CategoryModel, CategoryRequest, Guid>, ICategoryService
    {
        public CategoryService(IRepository<Category> repository, IMapper mapper) : base(repository, mapper)
        {
        }

        public Task<CategoryModel[]> GetCategoriesByUserId(Guid userId)
        {
            return Repository.GetAll()
                             .Where(q => q.UserId == userId)
                             .ProjectTo<CategoryModel>(Mapper.ConfigurationProvider)
                             .ToArrayAsync();
        }
    }
}