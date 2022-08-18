using CryptoZAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CryptoZAPI.Controllers
{
    [Route("history")]
    [ApiController]
    public class HistoryController : ControllerBase
    {

        // Logging
        private readonly ILogger<HistoryController> _logger;

        public HistoryController(ILogger<HistoryController> logger)
        {
            _logger = logger;
        }

        // GetAll per user + last x number
        // Post 


        // GET
        [HttpGet("{idUser}")]
        public IEnumerable<History> GetAll(int idUser)
        {
            // Find History for a user
            return new List<History>();
        }

        // POST
        [HttpPost]
        public History Post([FromBody] History history)
        {
            return history;
        }

    }
}
