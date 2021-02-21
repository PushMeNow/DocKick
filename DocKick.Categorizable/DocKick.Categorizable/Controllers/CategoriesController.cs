using System;
using System.Threading.Tasks;
using AutoMapper;
using DocKick.Dtos.Categories;
using DocKick.Services.Categories;
using Microsoft.AspNetCore.Mvc;

namespace DocKick.Categorizable.Controllers
{
    public class CategoriesController : BaseApiController
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService, IMapper mapper) : base(mapper)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<CategoryModel[]> GetCategories()
        {
            return await _categoryService.GetCategoriesByUserId(UserId);
        }

        [HttpGet("category-tree")]
        public async Task<CategoryModel[]> GetCategoryTree()
        {
            return await _categoryService.GetCategoryTreeByUserId(UserId);
        }

        [HttpPost]
        public async Task<CategoryModel> Create([FromBody] CategoryRequest request)
        {
            CheckValidation();

            var model = Mapper.Map<CategoryModel>(request);
            
            SetUserId(model);

            return await _categoryService.Create(model);
        }

        [HttpPut("{categoryId:Guid}")]
        public async Task<CategoryModel> Update([FromRoute] Guid categoryId, [FromBody] CategoryRequest request)
        {
            CheckValidation();

            var model = Mapper.Map<CategoryModel>(request);
            
            SetUserId(model);

            return await _categoryService.Update(categoryId, model);
        }

        [HttpDelete("{categoryId:Guid}")]
        public async Task Delete([FromRoute] Guid categoryId)
        {
            CheckValidation();
            
            await _categoryService.Delete(categoryId);
        }

        private void SetUserId(CategoryModel model)
        {
            model.UserId = UserId;
        }
    }
}