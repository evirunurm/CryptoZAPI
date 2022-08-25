using CryptoZAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Repo;
using System.Collections.Generic;

namespace CryptoZAPI.Controllers
{
    [Route("history")]
    [ApiController]
    public class HistoryController : ControllerBase
    {

        // Logging
        private readonly ILogger<HistoryController> _logger;
        private readonly IRepository repository;

        public HistoryController(ILogger<HistoryController> logger, IRepository repository)
        {
            _logger = logger;
            this.repository = repository;
        }

        // GET
        [HttpGet("{idUser}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<History>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> GetAll(Guid idUser, int limit)
        {
            // Get all history where idUser == idUser, with limit limit, ordenador por fecha desc
            List<History>? histories;

            try
            {
                histories = await repository.GetAllHistoriesForUser(idUser, limit);
                if (histories.Count == 0) { 
                    return NoContent();
                }
            }
            catch (Exception e) // TODO: Change Exception type
            {
                Console.WriteLine(e);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, "Database couldn't be accessed"); ;
            }
            return Ok(histories);
        }

        // POST
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(History))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> Post([FromBody] History history)
        {
            History? newHistory;

            // Maybe it's easier if this is in Database layer
            history.Date = DateTime.Now;

            try
            {
                newHistory = await repository.CreateHistory(history);
                if (newHistory is null)
                {
                    return NotFound();
                }
            }
            catch (Exception e) // TODO: Change Exception type
            {
                Console.WriteLine(e.Message);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, "Database couldn't be accessed"); ;
            }
            return Ok(newHistory);
        }

    }
}
