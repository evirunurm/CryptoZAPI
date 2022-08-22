using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CryptoZAPI.Models
{
    public class Currency
    {
        [Key]
    //El id debería ser publico, private set
        public Guid Id { get; private set; } = Guid.NewGuid();
        [Required]
        [Index(IsUnique=true)]
        [MaxLength(10)]
        public string Code { get; set; }
        [Required]
        [MaxLength(25)]
        public string Name { get; set; }
        [Required]
        public double Price { get; set; }
        public DateTime PriceDate { get; set; }
        public string LogoUrl { get; set; }

        public Currency(string id, string name, double price, DateTime price_date, string logo_url)
        {
            Code = id;
            Name = name;
            Price = price;
            PriceDate = price_date;
            LogoUrl = logo_url;
        }
    }

}
