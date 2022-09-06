using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Models
{
    public class Country
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "You should provide a Name value.")]
        [MaxLength(30, ErrorMessage = "The Name shouldn't have more than 30 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "You should provide a CountryCode value.")]
        [Index(IsUnique = true)]
        [MaxLength(2, ErrorMessage = "The CountryCode shouldn't have more than 2 characters.")]
        public string CountryCode { get; set; }

        //Relations
        public List<User> Users { get; set; }


    }
}
