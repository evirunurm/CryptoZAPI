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
        public Guid Id { get; } = Guid.NewGuid();
        [Required]
        public int Origin { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int Destination { get; set; }
        [Required]
        public double Value { get; set; }
        [Required]
        public double Result { get; set; }
		public DateTime Date { get; set; }

		public History(int origin, int destination, double value, double result, DateTime date, int userId)
        {
            Origin = origin;
            Destination = destination;
            Value = value;
            Result = result;
            Date = date;
            UserId = userId;
        }
    }
}
