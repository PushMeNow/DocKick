using System;
using DocKick.Categorizable.Tests.Helpers;
using DocKick.Data.Repositories;
using DocKick.Entities.Categories;
using DocKick.Services.Categories;

namespace DocKick.Categorizable.Tests.Services.Fixtures
{
    public class CategoryServiceFixture : BaseServiceFixture<CategoryService>
    {
        private const string UpdateCategoryName = "Test Updating category";

        public Guid UpdateCategoryId { get; set; }
        public Guid DeleteCategoryId { get; set; }
        public Guid NewParentCategoryId { get; set; }
        public Guid TestBlobUserId { get; } = new("6f722fbe-cd44-4d27-b0ab-f1e54f6c1b96");

        public override CategoryService CreateService()
        {
            var repo = new CategoryRepository(Context);

            return new CategoryService(repo, MapperHelper.Instance);
        }

        protected override void InitDatabase()
        {
            base.InitDatabase();

            UpdateCategoryId = Context.Categories.Add(new Category()
                                                      {
                                                          Name = UpdateCategoryName,
                                                          UserId = TestBlobUserId
                                                      })
                                      .Entity.CategoryId;

            DeleteCategoryId = Context.Categories.Add(new Category()
                                                      {
                                                          Name = "Deleting",
                                                          UserId = TestBlobUserId
                                                      })
                                      .Entity.CategoryId;

            NewParentCategoryId = Context.Categories.Add(new Category()
                                                         {
                                                             Name = "Parent",
                                                             UserId = TestBlobUserId
                                                         })
                                         .Entity.CategoryId;
        }
    }
}