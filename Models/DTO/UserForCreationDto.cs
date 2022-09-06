﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.DTO {
    public class UserForCreationDto {
        [Required(ErrorMessage = "You should provide a Name value.")]
        [MaxLength(64, ErrorMessage = "The Name shouldn't have more than 64 characters.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "You should provide a Email value.")]
        [MaxLength(320, ErrorMessage = "The Email shouldn't have more than 320 characters.")]
        [Index(IsUnique = true)]
        public string Email { get; set; }
        [Required(ErrorMessage = "You should provide a Password value.")]
        [MinLength(8, ErrorMessage = "The Password shouldn't have less than 8 characters.")]
        public string Password { get; set; }
        [Required]
        public int CountryId { get; set; }   
    }
}
