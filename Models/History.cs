using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CryptoZAPI.Models
{
	public class History
	{
        [Key]
        public int Id { get; set; }
        [Index(0)]
        [Name("origin")]
		public string Origin { get; set; }
        [Index(5)]
        [Name("userId")]
        public int UserId { get; set; }
        [Index(1)]
        [Name("destination")]
		public string Destination { get; set; }
        [Index(2)]
		[Name("value")]
		public double Value { get; set; }
        [Index(3)]
		[Name("result")]
		public double Result { get; set; }
        [Index(4)]
		[Name("date")]
		public DateTime Date { get; set; }

		public History(int id, string origin, string destination, double value, double result, DateTime date, int userId)
        {
            Id = id;
            Origin = origin;
            Destination = destination;
            Value = value;
            Result = result;
            Date = date;
            UserId = userId;
        }
    }
}
