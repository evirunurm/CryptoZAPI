using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CryptoZAPI.Models
{
	public class History
	{	
        // public Guid Id { get; set; } = Guid.NewGuid();

        [Index(0)]
        [Name("origin")]
		public string Origin { get; set; }
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

		public History(string origin, string destination, double value, double result, DateTime date)
		{
			Origin = origin;
			Destination = destination;
			Value = value;
			Result = result;
			Date = date;
		}
	}
}
