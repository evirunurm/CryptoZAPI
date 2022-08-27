using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models.DTO {
    public class HistoryForCreationDto {
        [Required]
        public string OriginCode { get; set; }
        public string? UserEmail { get; set; }
        [Required]
        public string DestinationCode { get; set; }
        [Required]
        public double Value { get; set; }
    }
}
