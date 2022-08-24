using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace CryptoZAPI.Models
{
    public class CurrencyDto
    {
        [Required]
        [Index(IsUnique=true)]
        [MaxLength(10)]
        [JsonPropertyName("id")]
        public string Code { get; set; }
        [Required]
        [MaxLength(25)]
        public string Name { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public DateTime PriceDate { get; set; }
        public string logo_url { get; set; }

     

    }

}
