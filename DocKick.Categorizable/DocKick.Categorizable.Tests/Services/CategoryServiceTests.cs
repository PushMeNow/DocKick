using System;
using System.Threading.Tasks;
using DocKick.Categorizable.Tests.Services.Fixtures;
using DocKick.Dtos.Categories;
using Xunit;

namespace DocKick.Categorizable.Tests.Services
{
    public class CategoryServiceTests
    {
        [Fact]
        public async Task Create_OK()
        {
            using var fixture = new CategoryServiceFixture();
            var service = fixture.CreateService();

            var request = new CategoryRequest
                          {
                              UserId = fixture.TestBlobUserId,
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
            using var fixture = new CategoryServiceFixture();
            var service = fixture.CreateService();

            var request = new CategoryRequest
                          {
                              UserId = fixture.TestBlobUserId,
                              Name = "Updated category"
                          };

            var result = await service.Update(fixture.UpdateCategoryId, request);

            Assert.NotNull(result);
            Assert.NotEqual(result.CategoryId, Guid.Empty);
            Assert.Equal(result.CategoryId, fixture.UpdateCategoryId);
            Assert.Equal(result.Name, request.Name);
            Assert.Equal(result.Name, (await fixture.Context.Categories.FindAsync(fixture.UpdateCategoryId))?.Name);
        }

        [Fact]
        public async Task Update_Parent_OK()
        {
            using var fixture = new CategoryServiceFixture();
            var service = fixture.CreateService();

            var request = new CategoryRequest
                          {
                              Name = "Test",
                              UserId = fixture.TestBlobUserId,
                              ParentId = fixture.NewParentCategoryId
                          };

            var result = await service.Update(fixture.UpdateCategoryId, request);
            var updatedCategory = await fixture.Context.Categories.FindAsync(fixture.UpdateCategoryId);

            Assert.NotNull(result);
            Assert.NotNull(result.ParentId);
            Assert.Equal(result.ParentId, fixture.NewParentCategoryId);
            Assert.Equal(updatedCategory.ParentId, fixture.NewParentCategoryId);
            Assert.NotNull(updatedCategory.Parent);
        }

        [Fact]
        public async Task Delete_OK()
        {
            using var fixture = new CategoryServiceFixture();
            var service = fixture.CreateService();

            await service.Delete(fixture.DeleteCategoryId);

            Assert.DoesNotContain(fixture.Context.Categories, q => q.CategoryId == fixture.DeleteCategoryId);
        }
    }
}