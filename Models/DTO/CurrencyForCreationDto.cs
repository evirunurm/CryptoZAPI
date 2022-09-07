using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace Models.DTO {
    public class CurrencyForCreationDto {
        [Required(ErrorMessage = "You should provide an Id value.")]
        [Index(IsUnique = true)]
        [MaxLength(10, ErrorMessage = "The Id shouldn't have more than 10 characters.")]
        public string Id { get; set; } = String.Empty;

        [Required(ErrorMessage = "You should provide a Name value.")]
        [MaxLength(25, ErrorMessage = "The Name shouldn't have more than 25 characters.")]
        public string Name { get; set; } = String.Empty;
        
        [Required(ErrorMessage = "You should provide a Price value.")]
        public double Price { get; set; }

        [Required(ErrorMessage = "You should provide a price_date value.")]
        public DateTime price_date { get; set; }
        public string logo_url { get; set; } = String.Empty;
    }
}
