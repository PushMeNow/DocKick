using System;
using DocKick.Data;
using Microsoft.EntityFrameworkCore;

namespace DocKick.Categorizable.Tests.Helpers
{
    public class InMemoryContextBuilder
    {
        public static CategorizableDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<CategorizableDbContext>().UseInMemoryDatabase(Guid.NewGuid()
                                                                                                        .ToString())
                                                                               .Options;

            return new CategorizableDbContext(options);
        }
    }
}