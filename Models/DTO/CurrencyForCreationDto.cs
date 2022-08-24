using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace Models.DTO {
    public class CurrencyForCreationDto {
        [Required(ErrorMessage = "You should provide a Code value.")]
        [Index(IsUnique = true)]
        [MaxLength(10)]
        public string Code { get; set; }
        [Required(ErrorMessage = "You should provide a Name value.")]
        [MaxLength(25)]
        public string Name { get; set; }
        [Required(ErrorMessage = "You should provide a Price.")]
        public double Price { get; set; }
        [Required(ErrorMessage = "You should provide a Price Date value.")]
        public DateTime PriceDate { get; set; }
        public string? LogoUrl { get; set; }
    }

}
