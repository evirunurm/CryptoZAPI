using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace Models.DTO {
    public class CountryForViewDto {
    

        [Required(ErrorMessage = "You should provide a Name value.")]
        [MaxLength(30, ErrorMessage = "The Name shouldn't have more than 30 characters.")]
        public string Name { get; set; } = String.Empty;

        [Required(ErrorMessage = "You should provide a CountryCode value.")]
        [Index(IsUnique = true)]
        [MaxLength(2, ErrorMessage = "The CountryCode shouldn't have more than 2 characters.")]
        public string CountryCode { get; set; } = String.Empty;


    }
}
