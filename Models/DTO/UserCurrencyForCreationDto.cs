using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.DTO {
    public class UserCurrencyForCreationDto {

        [Required(ErrorMessage = "You should provide a Name value.")]
        [MaxLength(64, ErrorMessage = "The Name shouldn't have more than 64 characters.")]
        public string Name { get; set; } = String.Empty;
        
        [Required(ErrorMessage = "You should provide an User Id")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "You should provide a Currency Code.")]
        public string CurrencyCode { get; set; } = String.Empty;        
    }
}
