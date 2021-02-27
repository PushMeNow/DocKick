using DocKick.Entities.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DocKick.Data.Configurations
{
    public class BlobLinkConfiguration : IEntityTypeConfiguration<BlobLink>
    {
        public void Configure(EntityTypeBuilder<BlobLink> builder)
        {
            builder.HasKey(q => q.BlobLinkId);

            builder.HasOne(q => q.Blob)
                   .WithOne(q => q.BlobLink)
                   .HasForeignKey<BlobLink>(q => q.BlobId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}