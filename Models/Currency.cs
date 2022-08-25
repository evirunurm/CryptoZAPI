using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace CryptoZAPI.Models
{
    public class Currency
    {
        [Key]
        public int Id { get; set; }
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
        
        public List<History> HistoriesOrigin{ get; set; }
        public List<History> HistoriesDestination { get; set; }

    }

}
