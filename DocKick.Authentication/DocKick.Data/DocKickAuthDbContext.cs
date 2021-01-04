using System;
using System.Reflection;
using DocKick.Entities.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DocKick.Data
{
    public class DocKickAuthDbContext : IdentityDbContext<User, Role, Guid>
    {
        public DocKickAuthDbContext(DbContextOptions<DocKickAuthDbContext> options) : base(options)
        {
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}