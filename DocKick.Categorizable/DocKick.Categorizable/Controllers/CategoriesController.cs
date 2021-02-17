using System;
using System.Threading.Tasks;
using DocKick.Dtos.Categories;
using DocKick.Services.Categories;
using Microsoft.AspNetCore.Mvc;

namespace DocKick.Categorizable.Controllers
{
    public class CategoriesController : BaseApiController
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<CategoryModel[]> GetCategories()
        {
            return await _categoryService.GetCategoriesByUserId(UserId);
        }

        [HttpPost]
        public async Task<CategoryModel> Create([FromBody]CategoryRequest request)
        {
            return await _categoryService.Create(request);
        }

        [HttpPut("categoryId:Guid")]
        public async Task<CategoryModel> Update([FromRoute] Guid categoryId, [FromBody]CategoryRequest request)
        {
            return await _categoryService.Update(categoryId, request);
        }

        [HttpDelete("{categoryId:Guid}")]
        public async Task Delete([FromRoute] Guid categoryId)
        {
            await _categoryService.Delete(categoryId);
        }
    }
}