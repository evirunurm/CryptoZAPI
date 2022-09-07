using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.DTO {
    public class UserForUpdateDto {

        [Required(ErrorMessage = "You should provide a Name value.")]
        [MaxLength(64)]
        public string Name { get; set; } = String.Empty;

        [Required(ErrorMessage = "You should provide a Password value.")]
        [MinLength(8, ErrorMessage = "The Password shouldn't have less than 8 characters.")]
        public string Password { get; set; } = String.Empty;

        [Required(ErrorMessage = "You should provide a New Password value.")]
        [MinLength(8, ErrorMessage = "The Password shouldn't have less than 8 characters.")]
        public string NewPassword { get; set; } = String.Empty;

        [Required(ErrorMessage = "You should provide a Country Code.")]
        [StringLength(2,MinimumLength = 2, ErrorMessage = "The Country Code must have exactly 2 characters.")]
        public string CountryCode { get; set; } = String.Empty;
    }
}
