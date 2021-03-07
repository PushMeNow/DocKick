using System;
using DocKick.Data.Repositories;
using DocKick.Entities.Categories;

namespace DocKick.Categorizable.Tests.Repositories.Fixtures
{
    internal class CategoryRepositoryFixture : BaseRepositoryFixture<CategoryRepository>
    {
        public Guid UserId { get; } = Guid.NewGuid();

        public Guid UpdateCategoryId { get; private set; }

        public Guid DeleteCategoryId { get; private set; }

        protected override CategoryRepository CreateRepository()
        {
            return new(Context);
        }

        protected override void InitDatabase()
        {
            UpdateCategoryId = Context.Categories.Add(new Category()
                                                      {
                                                          Name = "Updating category",
                                                          UserId = UserId
                                                      })
                                      .Entity.CategoryId;

            DeleteCategoryId = Context.Categories.Add(new Category()
                                                      {
                                                          Name = "Deleting category",
                                                          UserId = UserId
                                                      })
                                      .Entity.CategoryId;

            Context.SaveChanges();
        }
    }
}