using Microsoft.EntityFrameworkCore;
using System;
using TunedIn.Models;
using TunedIn.Utilities;

namespace TunedIn.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<SongEntity> Songs { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={AppDataPaths.GetDatabaseFilePath()}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SongEntity>().HasKey(s => s.Id);
            base.OnModelCreating(modelBuilder);
        }
    }
}
