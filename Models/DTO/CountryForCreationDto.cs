using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace Models.DTO {
    public class CountryForCreationDto {
     
        [Required]
        [MaxLength(25)]
        public string name { get; set; }
        [Required]
        [MaxLength(2)]
        public string alpha2Code { get; set; }
    
    }
}
