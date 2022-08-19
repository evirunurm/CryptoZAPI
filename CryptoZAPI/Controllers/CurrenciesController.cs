using CryptoZAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NomixServices;

namespace CryptoZAPI.Controllers
{
	[Route("currencies")]
	[ApiController]
	public class CurrenciesController : ControllerBase
	{

		

		// Logging
		private readonly ILogger<CurrenciesController> _logger;
        private readonly INomics nomics;

        public CurrenciesController(ILogger<CurrenciesController> logger, INomics nomics)
		{
			_logger = logger;
            this.nomics = nomics;
        }

		// GET
		[HttpGet]
		public IEnumerable<Currency> GetAll()
		{
			
			// Cargar
            List<Currency> Currencies = nomics.getCurrencies();
			try
			{
				Currencies = new List<Currency>(); // Va a hacer algo

			} catch (Exception e) 
			{
				// Send a code 
				Console.WriteLine(e);
			}


			return Currencies;
		}


		[HttpGet("{id}")]
		public Currency? FindOne(int id)
		{

		   // Cargar
			List<Currency> Currencies = new List<Currency>();
			Currency? currency = null;

			try
			{
				currency = Currencies.First(currency => currency.Id == id);
			} catch (ArgumentNullException e)
			{
				// Send a code 
				Console.WriteLine(e.Message);
			}

			return currency;
		}

		// PUT
		[HttpPut("{id}")]
		public Currency Put(int id, [FromBody] Currency c)
		{
			// hara cosas
			// Cambiar name of Currency c where id == id
			return c;
		}
	}
}
