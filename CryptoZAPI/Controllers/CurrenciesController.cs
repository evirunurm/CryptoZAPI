using CryptoZAPI.Models;
using Microsoft.AspNetCore.Mvc;
using NomixServices;
using Repo;

namespace CryptoZAPI.Controllers {
    [Route("currencies")]
    [ApiController]
    public class CurrenciesController : ControllerBase {

        // Logging
        private readonly ILogger<CurrenciesController> _logger;
        private readonly INomics nomics;
        private readonly IRepository repository;
        private DateTime lastRequested;

        // Optimización  
        //private readonly int lastRequestMinuteOffset = 10;
        

        public CurrenciesController(ILogger<CurrenciesController> logger, INomics nomics, IRepository repository) {
            this._logger = logger;
            this.nomics = nomics;
            this.repository = repository;
            this.lastRequested = DateTime.Now.Date;

            //

        }

        // GET currencies
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Currency>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> GetAll() {

            // Optimización            
            //if (DateTime.Now.Date.CompareTo(lastRequested) > 0) { 

            if (!lastRequested.Equals(DateTime.Now.Date)) {
                bool updated = await UpdateDatabase();
                if (!updated) {
                    return StatusCode(StatusCodes.Status503ServiceUnavailable, "There's been a problem with our database.");
                }
            }

            List<Currency>? Currencies;

            try {
                Currencies = repository.GetAllCurrencies();
                Currencies = new List<Currency>();
                Currencies.Add(new Currency(
                 "USD",
                 "Dollar",
                 1,
                  DateTime.Parse("2022-08-02T00:00:00Z"),
                 ""
                ));
                if (Currencies.Count == 0) {
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

        [HttpPost("/convert")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(double))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(double))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetConversion(string codeOrigin, string codeDestination, double value, bool save) {
            // conversion = result
            double result = 0;
            
            // if (save) pues guarda history
            //		History h;
            // if catch --> Forbidden();

            return Ok(result);


            /*OPCION DOS
			 * Desde el frontend llamar primero a GetConversion: se devuelve la conversion
			 * Si está logeado y el check está seleccionado: llama a otra función PostHistory, que almacene en el historial
			 */

        }

        /* Opción dos		 		 
		 
        [HttpPost("/convert")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(double))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetConversion(string codeOrigin, string codeDestination, double value) {
            // conversion = result
            double result = 0;  

            return Ok(result);
        }

        
		[HttpPut]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(¿History?))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

		public async Task<IActionResult> SaveConversion(string userId, string codeOrigin, string codeDestination, double converted_value) {
			// conversion = result

			History h;

            if (userId no existe) -> return 403
			
            if (!SaveToDB()) -> return error_no_se_ha_podido_crear

			return Ok(result);

        }
		*/


        // GET currencies/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Currency))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> FindOne(int id) {
            if (!lastRequested.Equals(DateTime.Now.Date)) {
                bool updated = await UpdateDatabase();
                if (!updated) {
                    return StatusCode(StatusCodes.Status503ServiceUnavailable, "There's been a problem with our database.");
                }
            }

            Currency? currency;

            try {
                currency = repository.GetOneCurrency(id);
            }
            catch (ArgumentNullException e) {
                Console.WriteLine(e.Message);
                return NotFound();
            }

            return Ok(currency);
        }

        // PUT
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Currency))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(int id, [FromBody] Currency c) {
            Currency newCurrency = null;
            try {
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



        private async Task<bool> UpdateDatabase() {
            try {
                List<Currency> CurrenciesToAdd = await nomics.getCurrencies();
                // TODO: Update currencies in database. + await 
                this.lastRequested = DateTime.Now.Date;

                // Optimización
                //this.lastRequested = DateTime.Now.Date.AddMinutes(lastRequestMinuteOffset);

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
