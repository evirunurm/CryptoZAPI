using CryptoZAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NomixServices;
using Repository;

namespace CryptoZAPI.Controllers
{
	[Route("currencies")]
	[ApiController]
	public class CurrenciesController : ControllerBase
	{

		// Logging
		private readonly ILogger<CurrenciesController> _logger;
        private readonly INomics nomics;
		private readonly IRepository repository;
		private DateTime lastRequested;

        public CurrenciesController(ILogger<CurrenciesController> logger, INomics nomics, IRepository repository)
		{
			this._logger = logger;
            this.nomics = nomics;
			this.repository = repository;
			this.lastRequested = DateTime.Now.Date;
        }

		// GET currencies
		[HttpGet]
        public IActionResult GetAll()
		{
			if (!lastRequested.Equals(DateTime.Now.Date))
			{
				bool updated = UpdateDatabase();
                if (!updated)
				{
					// TODO: Couldn't update database
				}
            }

			List<Currency>? Currencies;

			try
			{
				Currencies = repository.GetAllCurrencies();
				// TODO: Send a code
				Console.WriteLine(Currencies.Count);
				if (Currencies.Count == 0)
				{
					return NoContent();
				}
            }
            catch (Exception e) // TODO: Change Exception type
            {
                // TODO: Send a code 
                Console.WriteLine(e);
                return null;
            }
			return Ok(Currencies);
		}

		// GET currencies/{id}
		[HttpGet("{id}")]
		public Currency? FindOne(int id)
		{
			if (!lastRequested.Equals(DateTime.Now.Date))
			{
                bool updated = UpdateDatabase();
                if (!updated)
                {
                    // TODO: Couldn't update database
                }
			}
			
			Currency? currency;

			try
			{
				currency = repository.GetOneCurrency(id);
                // TODO: Send a code 
            }
            catch (ArgumentNullException e)
			{
				// TODO: Send a code 
				Console.WriteLine(e.Message);
				return null;
			}

			return currency;
		}

		// PUT
		[HttpPut("{id}")]
		public Currency? Put(int id, [FromBody] Currency c)
		{
			try
			{
				repository.ModifyCurrency(id, c);
                // TODO: Send a code 
            }
            catch (Exception e) // TODO: Change Exception type
            {
                // TODO: Send a code 
                Console.WriteLine(e.Message);
				return null;
			}
			// hara cosas
			// Cambiar name of Currency c where id == id
			return c;
		}

		private bool UpdateDatabase()
		{
            try
            {
                List<Currency> CurrenciesToAdd = nomics.getCurrencies();
                // TODO: Update currencies in database.
                this.lastRequested = DateTime.Now.Date;
				return true;
            }
            catch (Exception e) // TODO: Change Exception type
            {
                Console.WriteLine(e.Message);
				return false;
            }
        }
	}
}
