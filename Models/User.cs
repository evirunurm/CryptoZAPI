using CryptoZAPI.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

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

        [Required]
        public int CountryId { get; set; }

        public Country Country { get; set; }

        // Relations
        public List<History> Histories { get; set; }

        
        
    }
}
