using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DocKick.Data.Repositories;
using DocKick.Dtos.Categories;
using DocKick.Entities.Category;
using DocKick.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DocKick.Services
{
    public class CategoryService
    {
        private readonly IRepository<Category> _repository;
        private readonly IMapper _mapper;

        public CategoryService(IRepository<Category> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IReadOnlyCollection<CategoryModel>> GetCategoriesByUserId(Guid userId)
        {
            var categories = _repository.GetAll()
                                        .Where(q => q.UserId == userId)
                                        .ProjectTo<CategoryModel>(_mapper.ConfigurationProvider);

            return await categories.ToArrayAsync();
        }

        public async Task<CategoryModel> GetCategory(Guid categoryId)
        {
            var category = await _repository.GetById(categoryId);

            ExceptionHelper.ThrowNotFoundIfNull(category, "Category");

            var mapped = _mapper.Map<CategoryModel>(category);

            return mapped;
        }

        public async Task<CategoryModel> CreateCategory(CategoryModel model)
        {
            ExceptionHelper.ThrowArgumentNullIfNull(model, nameof(model));

            var category = _mapper.Map<Category>(model);

            await _repository.Create(category);
            await _repository.Save();

            var mapped = _mapper.Map<CategoryModel>(category);

            return mapped;
        }

        public async Task<CategoryModel> UpdateCategory(CategoryModel model)
        {
            var category = await _repository.GetById(model.CategoryId);

            ExceptionHelper.ThrowNotFoundIfNull(category, "Category");

            category = _mapper.Map(model, category);

            _repository.Update(category);
            await _repository.Save();

            var mapped = _mapper.Map<CategoryModel>(category);

            return mapped;
        }

        public async Task DeleteCategory(Guid categoryId)
        {
            await _repository.Delete(categoryId);
            await _repository.Save();
        }
    }
}