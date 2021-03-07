using System;
using System.Threading.Tasks;
using DocKick.Categorizable.Tests.Helpers;
using DocKick.Data;

namespace DocKick.Categorizable.Tests.Repositories.Fixtures
{
    public abstract class BaseRepositoryFixture<TRepository> : IDisposable
    {
        private TRepository _repository;
        
        protected BaseRepositoryFixture()
        {
            Context = InMemoryContextBuilder.CreateContext();
            
            InitDatabase();
        }

        public CategorizableDbContext Context { get; }

        public TRepository Repository => _repository ??= CreateRepository();

        public Task SaveContext()
        {
            return Context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Context?.Dispose();
        }

        protected abstract TRepository CreateRepository();

        protected abstract void InitDatabase();
    }
}