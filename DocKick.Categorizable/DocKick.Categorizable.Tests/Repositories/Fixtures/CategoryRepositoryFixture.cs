using DocKick.Data.Repositories;

namespace DocKick.Categorizable.Tests.Repositories.Fixtures
{
    public class CategoryRepositoryFixture : BaseRepositoryFixture<CategoryRepository>
    {
        public override CategoryRepository CreateRepository()
        {
            return new(Context);
        }

        public override void InitDatabase()
        {
            
        }
    }
}