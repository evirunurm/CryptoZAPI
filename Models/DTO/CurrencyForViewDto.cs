using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace Models.DTO {
    public class CurrencyForCreationDto {
        [Required]
        [Index(IsUnique = true)]
        [MaxLength(10)]
        public string Id { get; set; }
        [Required]
        [MaxLength(25)]
        public string Name { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public DateTime price_date { get; set; }
        public string? logo_url { get; set; }
    }
}
