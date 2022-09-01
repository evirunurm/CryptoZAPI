using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Models
{
    public class Country
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [MaxLength(2)]
        public string CountryCode { get; set; }

        //Relations
        public List<User> Users { get; set; }


    }
}
