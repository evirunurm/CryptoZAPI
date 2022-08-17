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
        [HttpGet(Name = "GetHistories")]
        public IEnumerable<History> GetAll(int idUser)
        {
            // Find History for a user
            return new List<History>();
        }

        // [HttpGet(Name = "GetHistory")]
        // public History Get(int id)
         //{
          //   return null;
         //}

        // POST
        [HttpPost(Name = "PostHistory")]
        public History Post(History history)
        {
            return null;
        }

    }
}
