using DocKick.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DocKick.Data.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(q => q.RefreshTokenId);

            builder.HasOne(q => q.User)
                   .WithMany(q => q.RefreshTokens)
                   .HasForeignKey(q => q.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}