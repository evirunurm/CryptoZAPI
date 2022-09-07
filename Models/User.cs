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

        [Required(ErrorMessage = "You should provide a Name value.")]
        [MaxLength(64, ErrorMessage = "The Name shouldn't have more than 64 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "You should provide a Email value.")]
        [MaxLength(320, ErrorMessage = "The Email shouldn't have more than 320 characters.")]
        [Index(IsUnique = true)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "You should provide a Password value.")]
        [MinLength(8, ErrorMessage = "The Password shouldn't have less than 8 characters.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "You should provide a Brithdate value.")]
        public DateTime Birthdate { get; set; } = new DateTime();

        /* -- Relations -- */
        public int CountryId { get; set; }
        public Country? Country { get; set; }
        public List<History> Histories { get; set; } = new List<History>();

        
     public void UpdateFromUser(User? user) {
            if (user == null)
                throw new ArgumentNullException();

            this.Name = user.Name;
            this.Password = user.Password;
            this.Country = user.Country;
            this.CountryId = user.CountryId;
        }   
    }
}
