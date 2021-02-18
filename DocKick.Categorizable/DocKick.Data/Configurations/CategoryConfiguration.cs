using DocKick.Entities.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DocKick.Data.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(q => q.CategoryId);

            builder.HasOne(q => q.Parent)
                   .WithMany(q => q.Children)
                   .IsRequired(false)
                   .HasForeignKey(q => q.ParentId);

            builder.HasMany(q => q.Blobs)
                   .WithOne(q => q.Category)
                   .HasForeignKey(q => q.CategoryId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(q => q.Name)
                   .HasMaxLength(100);

            builder.HasIndex(q => new { q.UserId, q.Name, q.ParentId })
                   .IsUnique();
        }
    }
}