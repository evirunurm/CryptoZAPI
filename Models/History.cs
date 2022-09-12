using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CryptoZAPI.Models {
    public class History {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "You should provide a Value value.")]
        public double Value { get; set; }

        [Required(ErrorMessage = "You should provide a Result value.")]
        public double Result { get; set; }
        public DateTime Date { get; set; }

        // Relations
        public User? User { get; set; }
        public Currency Origin { get; set; } = new Currency();
        public Currency Destination { get; set; } = new Currency();

        [Required(ErrorMessage = "You should provide a UserId value.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "You should provide a OriginId value.")]
        public int OriginId { get; set; }

        [Required(ErrorMessage = "You should provide a DestinationId value.")]
        public int DestinationId { get; set; }



    }
}
