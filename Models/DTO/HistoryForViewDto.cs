using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models.DTO {
    public class HistoryForViewDto {
        [Required]
        public string OriginCode { get; set; }       
        [Required]
        public string DestinationCode { get; set; }
        [Required]
        public double Value { get; set; }
        [Required]
        public double Result { get; set; }
        [Required]
        public DateTime Date { get; set; }
    }
}
