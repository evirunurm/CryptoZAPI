using AutoMapper;
using CryptoZAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTO;
using Repo;
using System.Collections.Generic;

namespace CryptoZAPI.Controllers {
    [Route("history")]
    [ApiController]
    public class HistoryController : ControllerBase {

        // Logging
        private readonly ILogger<HistoryController> _logger;
        private readonly IRepository repository;
        private readonly IMapper _mapper;

        public HistoryController(ILogger<HistoryController> logger, IRepository repository, IMapper mapper) {
            _logger = logger;
            this.repository = repository;
            this._mapper = mapper;
        }

        // GET
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<HistoryForViewDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> GetAll(string emailUser, int limit = 0) {
            // Get all history where idUser == idUser, with limit limit, ordenador por fecha desc

            try {

                List<HistoryForViewDto> histories;

                int userId = (await repository.GetUserByEmail(emailUser)).Id;
                histories = _mapper.Map<List<HistoryForViewDto>>(await repository.GetAllHistoriesForUser(userId, limit));

                if (histories.Count == 0) {
                    return NoContent();
                }

                return Ok(histories);

            }
            catch (Exception e) // TODO: Change Exception type
            {
                Console.WriteLine(e);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, "Database couldn't be accessed"); ;
            }
        }

        // POST
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HistoryForViewDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> Post([FromBody] HistoryForCreationDto history) {
            try {

                History historyMapped = _mapper.Map<History>(history);
                historyMapped.Origin = await repository.GetOneCurrency(history.OriginCode);
                historyMapped.Destination = await repository.GetOneCurrency(history.DestinationCode);
                historyMapped.Result = Utils.Conversion.Convert(historyMapped.Origin, historyMapped.Destination, historyMapped.Value);

                if (history.UserEmail != "") {
                    historyMapped.User = await repository.GetUserByEmail(history.UserEmail);
                }

                HistoryForViewDto historyDto = _mapper.Map<HistoryForViewDto>(await repository.CreateHistory(historyMapped));

                return Ok(historyDto);

            }
            catch (Exception e) // TODO: Change Exception type
            {
                Console.WriteLine(e.Message);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, "Database couldn't be accessed"); ;
            }
        }

    }
}
