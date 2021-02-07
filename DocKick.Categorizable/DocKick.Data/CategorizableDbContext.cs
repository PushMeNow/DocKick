using System.Reflection;
using DocKick.Entities.Blobs;
using DocKick.Entities.Categories;
using Microsoft.EntityFrameworkCore;

namespace DocKick.Data
{
    public class CategorizableDbContext : DbContext
    {
        public CategorizableDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<BlobContainer> BlobContainers { get; set; }
        public DbSet<Blob> Blobs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("DocKick.Data"));
        }
    }
}