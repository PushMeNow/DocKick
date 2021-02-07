using DocKick.Entities.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DocKick.Data.Configurations
{
    public class BlobConfiguration : IEntityTypeConfiguration<Blob>
    {
        public void Configure(EntityTypeBuilder<Blob> builder)
        {
            builder.HasKey(q => q.BlobId);
        }
    }
}