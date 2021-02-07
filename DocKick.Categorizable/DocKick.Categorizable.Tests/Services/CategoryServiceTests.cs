﻿using System;
using System.Linq;
using System.Threading.Tasks;
using DocKick.Categorizable.Tests.Services.Fixtures;
using DocKick.Dtos.Categories;
using Xunit;

namespace DocKick.Categorizable.Tests.Services
{
    public class CategoryServiceTests : IClassFixture<CategoryServiceFixture>
    {
        private readonly CategoryServiceFixture _fixture;

        public CategoryServiceTests(CategoryServiceFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Create_OK()
        {
            var service = _fixture.CreateService();

            var request = new CategoryRequest
                          {
                              Name = "Test Category"
                          };

            var result = await service.Create(request);

            Assert.NotNull(result);
            Assert.NotEqual(result.CategoryId, Guid.Empty);
            Assert.Equal(result.Name, request.Name);
        }

        [Fact]
        public async Task Update_OK()
        {
            var service = _fixture.CreateService();

            var request = new CategoryRequest
                          {
                              Name = "Updated category"
                          };

            var result = await service.Update(_fixture.UpdateCategoryId, request);

            Assert.NotNull(result);
            Assert.NotEqual(result.CategoryId, Guid.Empty);
            Assert.Equal(result.CategoryId, _fixture.UpdateCategoryId);
            Assert.Equal(result.Name, request.Name);
            Assert.Equal(result.Name, (await _fixture.Context.Categories.FindAsync(_fixture.UpdateCategoryId))?.Name);
        }

        [Fact]
        public async Task Update_Parent_OK()
        {
            var service = _fixture.CreateService();

            var result = await service.UpdateParent(_fixture.UpdateCategoryId, _fixture.NewParentCategoryId);
            var updatedCategory = await _fixture.Context.Categories.FindAsync(_fixture.UpdateCategoryId);

            Assert.NotNull(result);
            Assert.Equal(result.ParentId, _fixture.NewParentCategoryId);
            Assert.Equal(updatedCategory.ParentId, _fixture.NewParentCategoryId);
            Assert.NotNull(updatedCategory.Parent);
        }

        [Fact]
        public async Task Delete_OK()
        {
            var service = _fixture.CreateService();

            await service.Delete(_fixture.DeleteCategoryId);

            Assert.DoesNotContain(_fixture.Context.Categories, q => q.CategoryId == _fixture.DeleteCategoryId);
        }
    }
}