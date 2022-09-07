using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models.DTO {
    public class HistoryForViewDto {
        [Required(ErrorMessage = "You should provide a OriginCode value.")]
        public string OriginCode { get; set; } = String.Empty;
        [Required(ErrorMessage = "You should provide a DestinationCode value.")]
        public string DestinationCode { get; set; } = String.Empty;
        [Required(ErrorMessage = "You should provide a Value value.")]
        public double Value { get; set; }
        [Required(ErrorMessage = "You should provide a Result value.")]
        public double Result { get; set; }
        [Required(ErrorMessage = "You should provide a Date value.")]
        public DateTime Date { get; set; }
    }
}
