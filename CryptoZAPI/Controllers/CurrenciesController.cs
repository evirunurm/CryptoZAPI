using CryptoZAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CryptoZAPI.Controllers
{
    [Route("currencies")]
    [ApiController]
    public class CurrenciesController : ControllerBase
    {
        // Logging
        private readonly ILogger<CurrenciesController> _logger;

        public CurrenciesController(ILogger<CurrenciesController> logger)
        {
            _logger = logger;
        }

        // GetAll
        // FindOne
        // Put



        // GET
        [HttpGet(Name = "GetCurrencies")]
        public IEnumerable<Currency> Get()
        {
            return new List<Currency>();
        }

         // PUT
        [HttpPut(Name = "PutCurrencies")]
        public Currency Put(int id, [FromBody] Currency c)
        {
            // hara cosas
            return c;
        }
    }
}
