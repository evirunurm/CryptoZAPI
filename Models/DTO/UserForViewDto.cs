using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.DTO {
    public class UserForViewDto {

        [Required]
        [MaxLength(64)]
        public string Name { get; set; }
        [Required]
        [MaxLength(320)]
        [Index(IsUnique = true)]
        public string Email { get; set; }
    }
}
