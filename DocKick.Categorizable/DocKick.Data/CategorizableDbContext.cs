﻿using DocKick.Entities.Category;
using Microsoft.EntityFrameworkCore;

namespace DocKick.Data
{
    public class CategorizableDbContext : DbContext
    {
        public CategorizableDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
    }
}