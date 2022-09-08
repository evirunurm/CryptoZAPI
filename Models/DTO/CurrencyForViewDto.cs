using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace Models.DTO {
    public class CurrencyForViewDto {
        public string Code { get; set; } = String.Empty;
        public string Name { get; set; } = String.Empty;
        public double Price { get; set; }
        public DateTime PriceDate { get; set; }
        public string LogoUrl { get; set; } = String.Empty;
    }

}
