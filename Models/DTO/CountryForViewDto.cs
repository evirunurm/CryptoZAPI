using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace Models.DTO {
    public class CountryForViewDto {
        public string Name { get; set; } = String.Empty;
        public string CountryCode { get; set; } = String.Empty;


    }
}
