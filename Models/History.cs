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
        public Guid Id { get; } = Guid.NewGuid();
        [Required]
        public Guid Origin { get; }
        [Required]
        public Guid UserId { get; }
        [Required]
        public Guid Destination { get; }
        [Required]
        public double Value { get; set; }
        [Required]
        public double Result { get; set; }
		public DateTime Date { get; set; }

		public History(Guid origin, Guid destination, double value, double result, DateTime date, Guid userId)
        {
            Origin = origin;
            Destination = destination;
            Value = value;
            Result = result;
            Date = date;
            UserId = userId;
        }

        public History(Currency origin, Currency destination, double value, double result, DateTime date, User user) {
            Origin = origin.Id_old;
            Destination = destination.Id_old;
            Value = value;
            Result = result;
            Date = date;
            UserId = user.Id;
        }

        public History(Guid origin, Guid destination, double value, double result, DateTime date, User user) {
            Origin = origin;
            Destination = destination;
            Value = value;
            Result = result;
            Date = date;
            UserId = user.Id;
        }
    }
}
