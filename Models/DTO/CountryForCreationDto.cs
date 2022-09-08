using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace Models.DTO {
    public class CountryForCreationDto {
     
        [Required(ErrorMessage = "You should provide a Name value.")]
        [MaxLength(25, ErrorMessage = "The name shouldn't have more than 25 characters.")]
        public string name { get; set; } = String.Empty;

        [Required(ErrorMessage = "You should provide a alpha2Code value.")]
        [MaxLength(2, ErrorMessage = "The alpha2Code shouldn't have more than 2 characters.")]
        public string alpha2Code { get; set; } = String.Empty;

    }
}