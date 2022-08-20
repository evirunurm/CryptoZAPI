using CryptoZAPI.Models;
using Microsoft.AspNetCore.Mvc;
using NomixServices;
using Repo;

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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Currency>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> GetAll()
		{
			if (!lastRequested.Equals(DateTime.Now.Date))
			{
				bool updated = await UpdateDatabase();
            }

			List<Currency>? Currencies;

            try
			{
				Currencies = repository.GetAllCurrencies();
				if (Currencies.Count == 0)
				{
					return NoContent();
				}
            }
            catch (Exception e) // TODO: Change Exception type
            {
                Console.WriteLine(e);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, "Database couldn't be accessed"); ;
            }
			return Ok(Currencies);
		}

		// GET currencies/{id}
		[HttpGet("{id}")]
		public async Task<IActionResult> FindOne(int id)
		{
			if (!lastRequested.Equals(DateTime.Now.Date))
			{
                bool updated = await UpdateDatabase();
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

			return Ok(currency);
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

		private async Task<bool> UpdateDatabase()
		{
            try
            {
                List<Currency> CurrenciesToAdd = nomics.getCurrencies(); // add await
                // TODO: Update currencies in database. + await 
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
