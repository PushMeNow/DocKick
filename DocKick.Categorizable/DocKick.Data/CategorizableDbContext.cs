using System.Reflection;
using DocKick.Data.Configurations;
using DocKick.Entities.Category;
using Microsoft.EntityFrameworkCore;

namespace DocKick.Data
{
    public class CategorizableDbContext : DbContext
    {
        public CategorizableDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("DocKick.Data"));
        }
    }
}