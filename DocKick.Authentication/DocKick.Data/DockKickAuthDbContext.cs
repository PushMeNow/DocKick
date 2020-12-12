using DocKick.Data.Entities.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DocKick.Data
{
    public class DockKickAuthDbContext : IdentityDbContext<User>
    {
        public DockKickAuthDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}