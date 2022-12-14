using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Models;

namespace CryptoZAPI.Models {
    public class Country {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "You should provide a Name value.")]
        [MaxLength(30, ErrorMessage = "The Name shouldn't have more than 30 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "You should provide a CountryCode value.")]
        [Index(IsUnique = true)]
        [MaxLength(2, ErrorMessage = "The CountryCode shouldn't have more than 2 characters.")]
        public string CountryCode { get; set; } = String.Empty;

        //Relations
        public List<User> Users { get; set; } = new List<User>();

        public void UpdateFromCountry(Country? country) {
            if (country == null)
                throw new ArgumentNullException();

            this.Name = country.Name;          
        }
    }
}
