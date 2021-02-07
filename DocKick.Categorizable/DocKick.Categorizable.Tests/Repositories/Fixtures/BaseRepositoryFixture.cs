using DocKick.Categorizable.Tests.Helpers;
using DocKick.Data;

namespace DocKick.Categorizable.Tests.Repositories.Fixtures
{
    public abstract class BaseRepositoryFixture<TRepository>
    {
        protected BaseRepositoryFixture()
        {
            Context = InMemoryContextBuilder.CreateContext();
            
            InitDatabase();
        }

        public CategorizableDbContext Context { get; }

        public abstract TRepository CreateRepository();

        public abstract void InitDatabase();
    }
}