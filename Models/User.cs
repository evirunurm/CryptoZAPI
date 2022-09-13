using CryptoZAPI.Models;
using System;
using System.ComponentModel.DataAnnotations;
// using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Models {
    public class User : IdentityUser<int>
    {

        [Required(ErrorMessage = "You should provide a Name value.")]
        [MaxLength(64, ErrorMessage = "The Name shouldn't have more than 64 characters.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "You should provide a Brithdate value.")]
        public DateTime Birthdate { get; set; } = new DateTime();

        /* -- Relations -- */
        public int CountryId { get; set; }
        public Country Country { get; set; } = new Country();
        public List<History> Histories { get; set; } = new List<History>();
        public List<UserCurrency> UsersCurrencies { get; set; } = new List<UserCurrency>();

        public void UpdateFromUser(User? user) {
            if (user == null)
                throw new ArgumentNullException();

            this.FullName = user.FullName;
            this.Country = user.Country;
            this.CountryId = user.CountryId;
        }
    }
}
