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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Currency>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> GetAll()
		{
			if (!lastRequested.Equals(DateTime.Now.Date))
			{
				bool updated = await UpdateDatabase();
				if (!updated)
				{
					return StatusCode(StatusCodes.Status503ServiceUnavailable, "There's been a problem with our database.");

                }
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Currency))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> FindOne(int id)
		{
			if (!lastRequested.Equals(DateTime.Now.Date))
			{
                bool updated = await UpdateDatabase();
                if (!updated)
                {
                    return StatusCode(StatusCodes.Status503ServiceUnavailable, "There's been a problem with our database.");
                }
			}
			
			Currency? currency;

			try
			{
				currency = repository.GetOneCurrency(id);
            }
            catch (ArgumentNullException e)
			{
				Console.WriteLine(e.Message);
				return NotFound();
			}

			return Ok(currency);
		}

		// PUT
		[HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Currency))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(int id, [FromBody] Currency c)
		{
			Currency newCurrency = null;
			try
			{
                newCurrency = repository.ModifyCurrency(id, c); 
            }
            catch (Exception e) // TODO: Change Exception type
            {
                Console.WriteLine(e.Message);
				return NotFound();
			}
			// Cambiar name of Currency c where id == id
			return Ok(newCurrency);
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
