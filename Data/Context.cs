﻿using CryptoZAPI.Models;
using Microsoft.EntityFrameworkCore;
using Models;
// using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
	public class CryptoZContext : DbContext
	{
		public DbSet<Currency> Currencies { get; set; }
		public DbSet<History> History { get; set; }
		public DbSet<User> User { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=CryptoZdb;Trusted_Connection=True;MultipleActiveResultSets=true");
	
		}


		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
               .HasMany(u => u.Histories)
               .WithOne(h => h.User)
               .HasForeignKey(h => h.UserId);

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
        }
	}
}
