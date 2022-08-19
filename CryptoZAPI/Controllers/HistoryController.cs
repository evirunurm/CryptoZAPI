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

        // GET
        [HttpGet("{idUser}")]
        public IEnumerable<History> GetAll(int idUser, int limit)
        {
            // Get all history where idUser == idUser, with limit limit, ordenador por fecha desc
            return new List<History>();
        }

        // POST
        [HttpPost]
        public History Post([FromBody] History history)
        {
            // Add history to Histories table in database
            return history;
        }

    }
}
