using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CryptoZAPI.Models
{
    public class Currency
    {
        [Key]
        public string Id { get; set; }
        [Required]
        [MaxLength(25)]
        public string Name { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public DateTime PriceDate { get; set; }

        public string LogoUrl { get; set; }

        public Currency(string id, string name, double price, DateTime priceDate, string logoUrl)
        {
            Id = id;
            Name = name;
            Price = price;
            PriceDate = priceDate;
            LogoUrl = logoUrl;
        }
    }

}
