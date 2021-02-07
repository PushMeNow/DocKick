using System;
using DocKick.Categorizable.Tests.Helpers;
using DocKick.Data;

namespace DocKick.Categorizable.Tests.Services.Fixtures
{
    public abstract class BaseServiceFixture<TService> : IDisposable
    {
        public CategorizableDbContext Context { get; }

        protected BaseServiceFixture(bool createContext = true)
        {
            if (createContext)
            {
                Context = InMemoryContextBuilder.CreateContext();

                InitDatabase();

                Context.SaveChanges();
            }
        }

        public abstract TService CreateService();

        protected virtual void InitDatabase() { }

        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}