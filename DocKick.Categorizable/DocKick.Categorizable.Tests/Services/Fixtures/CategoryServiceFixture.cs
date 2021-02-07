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
                                                          Name = UpdateCategoryName
                                                      })
                                      .Entity.CategoryId;

            DeleteCategoryId = Context.Categories.Add(new Category()
                                                      {
                                                          Name = "Deleting"
                                                      })
                                      .Entity.CategoryId;

            NewParentCategoryId = Context.Categories.Add(new Category()
                                                         {
                                                             Name = "Parent"
                                                         })
                                         .Entity.CategoryId;
        }
    }
}