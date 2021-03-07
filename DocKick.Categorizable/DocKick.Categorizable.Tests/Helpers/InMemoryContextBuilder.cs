using System;
using DocKick.Data;
using Microsoft.EntityFrameworkCore;

namespace DocKick.Categorizable.Tests.Helpers
{
    public static class InMemoryContextBuilder
    {
        public static CategorizableDbContext CreateContext()
        {
            var builder = new DbContextOptionsBuilder<CategorizableDbContext>();

            var options = builder.UseInMemoryDatabase(Guid.NewGuid()
                                                          .ToString())
                                 .Options;

            return new CategorizableDbContext(options);
        }
    }
}