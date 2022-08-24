using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CryptoZAPI.Models
{
	public class HistoryDto
	{

        [Required]
        public int Origin { get; set;}
        [Required]
        public int UserId { get; set;}
        [Required]
        public int Destination { get; set;}
        [Required]
        public double Value { get; set; }
        [Required]
        public double Result { get; set; }
        [Required]
		public DateTime Date { get; set; }


       
    }
}
