using Models;
using Models.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models.DTO {

    [CurrenciesMustBeDifferent(ErrorMessage = "Origin currency and destination currency must be different.")]

    public class HistoryForCreationDto {
        [Required(ErrorMessage = "You should choose an origin currency.")]
        public string OriginCode { get; set; } = String.Empty;

        [Required(ErrorMessage = "You should choose a destination currency.")]
        public string DestinationCode { get; set; } = String.Empty;

        [RangeAttribute(0.0000000000001, Double.MaxValue, ErrorMessage = "Value for the amount must be a positive number")]
        [Required(ErrorMessage = "You should provide a positive Value for convertion.")]
        public double Value { get; set; }
    }
}



