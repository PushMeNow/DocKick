﻿using DocKick.Entities.Categories;
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
                   .WithOne(q => q.Child)
                   .IsRequired(false)
                   .HasForeignKey<Category>(q => q.ParentId)
                   .OnDelete(DeleteBehavior.SetNull);

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