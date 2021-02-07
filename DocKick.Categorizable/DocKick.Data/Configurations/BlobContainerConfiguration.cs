using DocKick.Entities.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DocKick.Data.Configurations
{
    public class BlobContainerConfiguration : IEntityTypeConfiguration<BlobContainer>
    {
        public void Configure(EntityTypeBuilder<BlobContainer> builder)
        {
            builder.HasKey(q => q.BlobContainerId);

            builder.HasMany(q => q.Blobs)
                   .WithOne(q => q.BlobContainer)
                   .HasForeignKey(q => q.BlobContainerId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(q => q.Name)
                   .IsUnique();
            
            builder.HasIndex(q => q.UserId)
                   .IsUnique();
        }
    }
}