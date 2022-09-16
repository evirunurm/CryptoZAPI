using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models.DTO {
    public class HistoryForViewDto {
        public string OriginCode { get; set; } = String.Empty;
        public string DestinationCode { get; set; } = String.Empty;
        public double Value { get; set; }
        public double Result { get; set; }
        public DateTime Date { get; set; }
        public double Factor { get; set; }
    }
}
