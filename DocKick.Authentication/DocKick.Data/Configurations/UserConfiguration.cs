using DocKick.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DocKick.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(q => q.City)
                   .HasMaxLength(100);

            builder.Property(q => q.Company)
                   .HasMaxLength(100);

            builder.Property(q => q.Country)
                   .HasMaxLength(100);

            builder.Property(q => q.Profession)
                   .HasMaxLength(100);

            builder.Property(q => q.FirstName)
                   .HasMaxLength(100);

            builder.Property(q => q.LastName)
                   .HasMaxLength(100);
        }
    }
}