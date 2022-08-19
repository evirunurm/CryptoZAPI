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
        [HttpGet]
        public IEnumerable<Currency> GetAll()
        {
            return new List<Currency>();
        }


        [HttpGet("{id}")]
        public Currency FindOne(int id)
        {
            return null;
        }

        // PUT
        [HttpPut("{id}")]
        public Currency Put(int id, [FromBody] Currency c)
        {
            // hara cosas
            return c;
        }
    }
}
