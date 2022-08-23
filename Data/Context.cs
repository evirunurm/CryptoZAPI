using CryptoZAPI.Models;
using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class CurrencyContext : DbContext
    {
        public DbSet<Currency> Currencies { get; set; }

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
