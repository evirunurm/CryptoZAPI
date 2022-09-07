using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace Models.DTO {
    public class CurrencyForViewDto {
        [Required(ErrorMessage = "You should provide a Code value.")]
        [Index(IsUnique = true)]
        [MaxLength(10, ErrorMessage = "The Code shouldn't have more than 10 characters.")]
        public string Code { get; set; } = String.Empty;

        [Required(ErrorMessage = "You should provide a Name value.")]
        [MaxLength(25, ErrorMessage = "The Name shouldn't have more than 25 characters.")]
        public string Name { get; set; } = String.Empty;

        [Required(ErrorMessage = "You should provide a Price.")]
        public double Price { get; set; }

        [Required(ErrorMessage = "You should provide a Price Date value.")]
        public DateTime PriceDate { get; set; }
        public string LogoUrl { get; set; } = String.Empty;
    }

}
