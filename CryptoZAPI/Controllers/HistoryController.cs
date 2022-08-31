using AutoMapper;
using CryptoZAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTO;
using Repo;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CryptoZAPI.Controllers {
    [Route("history")]
    [ApiController]
    public class HistoryController : ControllerBase {

        // Logging
        private readonly ILogger<HistoryController> _logger;
        private readonly IRepository<User> repositoryUser;
        private readonly IRepository<Currency> repositoryCurrency;
        private readonly IRepository<History> repository;
        private readonly IMapper _mapper;

        public HistoryController(ILogger<HistoryController> logger, IRepository<History> repositoryHistory, IRepository<User> repositoryUser, IRepository<Currency> repositoryCurrency, IMapper mapper) {
            _logger = logger;
            this.repositoryUser = repositoryUser ?? throw new ArgumentNullException(nameof(repositoryUser));
            this.repositoryCurrency = repositoryCurrency;
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
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

                var foundUsers = await repositoryUser.FindBy(u => u.Email == emailUser).ToListAsync();
                if (foundUsers.Count == 0)
                {
                    NotFound();
                } else if (foundUsers.Count > 1)
                {
                    // TODO: Error en el argumento
                }

                int userId = foundUsers[0].Id;
                histories = _mapper.Map<List<HistoryForViewDto>>(( await repository.FindBy(h => h.UserId == userId).ToListAsync() ));

                if (histories.Count == 0) {
                    return NoContent();
                }

                return Ok(histories);

            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, e.Message); ;
            }
            catch (OperationCanceledException e)
            {
                Console.WriteLine(e);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, e.Message); ;
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

                var foundListCurrencyOrigin = await repositoryCurrency.FindBy(c => c.Code == history.OriginCode).ToListAsync();
                var foundListCurrencyDestination = await repositoryCurrency.FindBy(c => c.Code == history.DestinationCode).ToListAsync();

                if (foundListCurrencyOrigin.Count == 0 || foundListCurrencyDestination.Count == 0)
                {
                   return NotFound(); // TODO: Edit
                }

                historyMapped.Origin = foundListCurrencyOrigin[0];
                historyMapped.Destination = foundListCurrencyDestination[0];

                historyMapped.Result = Utils.Conversion.Convert(historyMapped.Origin, historyMapped.Destination, historyMapped.Value);

                if (history.UserEmail != "") {

                    var foundUsers = await repositoryUser.FindBy(u => u.Email == history.UserEmail).ToListAsync();
                    if (foundUsers.Count == 0)
                    {
                        return NotFound(); // TODO: Edit
                    }

                    historyMapped.User = foundUsers[0];
                }

                HistoryForViewDto historyDto = _mapper.Map<HistoryForViewDto>(await repository.Create(historyMapped));
                await repository.SaveDB();

                return Ok(historyDto);
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, e.Message); ;
            }
            catch (OperationCanceledException e)
            {
                Console.WriteLine(e);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, e.Message); ;
            }
            catch (Exception e) // TODO: Change Exception type
            {
                Console.WriteLine(e.Message);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, "Database couldn't be accessed"); ;
            }
        }

    }
}
