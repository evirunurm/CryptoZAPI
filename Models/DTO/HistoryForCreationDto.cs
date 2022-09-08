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

        [Required(ErrorMessage = "You should provide a Email value.")]
        [MaxLength(320, ErrorMessage = "The Email shouldn't have more than 320 characters.")]
        public string? UserEmail { get; set; } = String.Empty;

        [Required(ErrorMessage = "You should choose a destination currency.")]
        public string DestinationCode { get; set; } = String.Empty;

        [Required(ErrorMessage = "You should provide a Value for convertion.")]
        public double Value { get; set; }
    }
}



