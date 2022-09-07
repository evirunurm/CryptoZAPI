using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.DTO {
    public class UserForViewDto {
        public string Name { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public string CountryName { get; set; } = String.Empty;
        public string CountryCode { get; set; } = String.Empty;
        public DateTime Birthdate { get; set; } = new DateTime();

    }
}
