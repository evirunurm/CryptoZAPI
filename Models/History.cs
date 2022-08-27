using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CryptoZAPI.Models
{
	public class History
	{
        [Key]
        public int Id { get; set; }
        [Required]
        public double Value { get; set; }
        [Required]
        public double Result { get; set; }
        public DateTime Date { get; set; }
      
        // Relations
        public User User { get; set; }
        [Required]
        public int UserId { get; set; }
        public Currency Origin { get; set; }
        [Required]
        public int OriginId { get; set; }
        public Currency Destination { get; set; }
        [Required]
        public int DestinationId { get; set; }



    }
}
