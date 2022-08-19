using CryptoZAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Repository;
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
        public IEnumerable<History>? GetAll(int idUser, int limit)
        {
            // Get all history where idUser == idUser, with limit limit, ordenador por fecha desc
            List<History>? histories;

            try
            {
                histories = repository.GetAllHistoriesForUser(idUser, limit);
                // TODO: Send a code 
            }
            catch (Exception e) // TODO: Change Exception type
            {
                // TODO: Send a code 
                Console.WriteLine(e.Message);
                return null;
            }
            return histories;
        }

        // POST
        [HttpPost]
        public History? Post([FromBody] History history)
        {
            History? newHistory;

            // Maybe it's easier if this is in Database layer
            history.Date = DateTime.Now;

            try
            {
                newHistory = repository.CreateHistory(history);
                // TODO: Send a code 
            }
            catch (Exception e) // TODO: Change Exception type
            {
                // TODO: Send a code 
                Console.WriteLine(e.Message);
                return null;
            }
            return newHistory;
        }

    }
}
