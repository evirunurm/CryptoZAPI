using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models.DTO {
    public class HistoryForViewDto {

        [Required]
        public string Origin { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public string Destination { get; set; }
        [Required]
        public double Value { get; set; }
        [Required]
        public double Result { get; set; }
        [Required]
        public DateTime Date { get; set; }



    }
}
