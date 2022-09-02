using CryptoZAPI.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Models;
// using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data {
    public class CryptoZContext : DbContext {
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<History> Histories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Country> Countries { get; set; }

        // DB Path
        private string DbPath = $"DB\\SQLite.DB";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {

            //optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=CryptoZdb;Trusted_Connection=True;MultipleActiveResultSets=true");
            optionsBuilder.UseSqlite($"Data Source={DbPath}");

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            /*
            modelBuilder.Entity<User>()
               .HasMany(u => u.Histories)
               .WithOne(h => h.User)
               .HasForeignKey(h => h.UserId);
			*/

            modelBuilder.Entity<History>()
                .HasOne(h => h.User)
                .WithMany(u => u.Histories)
                .HasForeignKey(h => h.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<History>()
                .HasOne(h => h.Origin)
                .WithMany(c => c.HistoriesOrigin)
                .HasForeignKey(h => h.OriginId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<History>()
                .HasOne(h => h.Destination)
                .WithMany(c => c.HistoriesDestination)
                .HasForeignKey(h => h.DestinationId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
              .HasOne(u => u.Country)
              .WithMany(c => c.Users)
              .HasForeignKey(u => u.CountryId)
              .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
