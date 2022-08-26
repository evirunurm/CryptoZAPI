using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.DTO {
    public class UserForCreationDto {

        [Required]
        [MaxLength(64)]
        public string Name { get; set; }
        [Required]
        [MaxLength(320)]
        [Index(IsUnique = true)]
        public string Email { get; set; }
        [Required]
        [MinLength(8)]
        public string Password { get; set; }
    }
}
