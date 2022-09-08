using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace CryptoZAPI.Models {
    public class Currency {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "You should provide a Code value.")]
        [Index(IsUnique = true)]
        [MaxLength(10, ErrorMessage = "The Code shouldn't have more than 10 characters.")]
        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "You should provide a Name value.")]
        [MaxLength(25, ErrorMessage = "The Name shouldn't have more than 25 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "You should provide a Price value.")]
        public double Price { get; set; }
        public DateTime PriceDate { get; set; }
        public string LogoUrl { get; set; } = string.Empty;

        // Relations
        public List<History> HistoriesOrigin { get; set; } = new List<History>();
        public List<History> HistoriesDestination { get; set; } = new List<History>();


        public void UpdateFromCurrency(Currency? currency) {
            if (currency == null)
                throw new ArgumentNullException();

            this.PriceDate = currency.PriceDate;
            this.Price = currency.Price;
            this.Name = currency.Name;
            this.LogoUrl = currency.LogoUrl;
        }
    }

}
