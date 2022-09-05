using Models;
using Models.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models.DTO {

    [CurrenciesMustBeDifferent(
         ErrorMessage = "Origin currency and destination currency must be different.")]


    public class HistoryForCreationDto {
  
        [Required(ErrorMessage = "You should choose a currency.")]
        public string OriginCode { get; set; }
        public string? UserEmail { get; set; }
   
        [Required(ErrorMessage = "You should choose a currency.")]
        public string DestinationCode { get; set; }
        [Required]
        public double Value { get; set; }
    }
}



