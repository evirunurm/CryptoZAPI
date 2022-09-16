using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.DTO
{
    public class UserForLoginDto
    {

        [Required(ErrorMessage = "You should provide a Email value.")]
        [MaxLength(320, ErrorMessage = "The Email shouldn't have more than 320 characters.")]
        public string Email { get; set; } = String.Empty;

        [Required(ErrorMessage = "You should provide a Password value.")]
        [MinLength(8, ErrorMessage = "The Password shouldn't have less than 8 characters.")]
        public string Password { get; set; } = String.Empty;
    }
}
