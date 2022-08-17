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

         // GET
        [HttpGet(Name = "GetCurrencies")]
        public IEnumerable<Currency> Get()
        {
            return new List<Currency>();
        }

         //[HttpGet(Name = "GetCurrency")]
        // public Currency Get(int id)
         //{
         //    return null;
         //}

         // PUT
        [HttpPut(Name = "PutCurrencies")]
        public Currency Get(int id, string newName)
        {
            return null;
        }
    }
}
