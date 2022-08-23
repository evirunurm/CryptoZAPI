using CryptoZAPI.Models;
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
            // var configuration = new ConfigurationBuilder()
                // .SetBasePath(Directory.GetCurrentDirectory())
                //.AddJsonFile("appsettings.json")
                //.Build();

            // var connectionString = configuration.GetConnectionString("AppDb");
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=CryptoZdb;Trusted_Connection=True;MultipleActiveResultSets=true");

       
        }
    }
}
