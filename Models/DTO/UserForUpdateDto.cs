using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.DTO {
    public class UserForUpdateDto {
        [MaxLength(64)]
        public string Name { get; set; }
        [Required(ErrorMessage = "You should provide a Email value.")]
        [MaxLength(320, ErrorMessage = "The Email shouldn't have more than 320 characters.")]
        [Index(IsUnique = true)]
        public string Email { get; set; }
        [Required(ErrorMessage = "You should provide a Password value.")]
        [MinLength(8, ErrorMessage = "The Password shouldn't have less than 8 characters.")]
        public string Password { get; set; }
        [MinLength(8, ErrorMessage = "The Password shouldn't have less than 8 characters.")]
        public string NewPassword { get; set; }
        public string CountryCode { get; set; }  
    }
}
