using System;
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
    }
}