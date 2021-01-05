using DocKick.Entities.Category;
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
                   .WithOne(q => q.Parent)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasForeignKey<Category>(q => q.ParentId);

            builder.Property(q => q.Name)
                   .HasMaxLength(100);

            builder.HasIndex(q => q.UserId)
                   .IsUnique();
        }
    }
}