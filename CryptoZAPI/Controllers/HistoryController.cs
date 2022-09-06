using AutoMapper;
using CryptoZAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTO;
using Repo;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace CryptoZAPI.Controllers {
    [Route("history")]
    [ApiController]
    public class HistoryController : ControllerBase {

        private readonly IRepository<User> repositoryUser;
        private readonly IRepository<Currency> repositoryCurrency;
        private readonly IRepository<History> repository;
        private readonly IMapper _mapper;

        public HistoryController(IRepository<History> repository, IRepository<User> repositoryUser, IRepository<Currency> repositoryCurrency, IMapper mapper) {
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
                    Log.Warning("No content found");
                    NotFound();
                } else if (foundUsers.Count > 1)
                {
                    // TODO: Error en el argumento
                }

                int userId = foundUsers[0].Id;
                List<History> history = await repository.FindBy(h => h.UserId == userId).ToListAsync();



                foreach (History h in history)
                {
                    var foundDestination = await repositoryCurrency.FindBy(c => c.Id == h.DestinationId).ToListAsync();
                    if (foundDestination.Count == 0)
                    {
                        Log.Warning("No content found");
                        NotFound();
                    }
                    else if (foundDestination.Count > 1)
                    {
                        // TODO: Error en el argumento
                    }
                    var foundOrigin = await repositoryCurrency.FindBy(c => c.Id == h.OriginId).ToListAsync();
                    if (foundOrigin.Count == 0)
                    {
                        Log.Warning("No content found");
                        NotFound();
                    }
                    else if (foundOrigin.Count > 1)
                    {
                        // TODO: Error en el argumento
                    }

                    h.Origin = foundOrigin[0];
                    h.Destination = foundDestination[0];
                }
                histories = _mapper.Map<List<HistoryForViewDto>>(history);

                if (histories.Count == 0) {
                    Log.Warning("No content found");
                    return NoContent();
                }

                return Ok(histories);

            }
            catch (ArgumentNullException e)
            {
                Log.Error(e.Message);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, e.Message); ;
            }
            catch (OperationCanceledException e)
            {
                Log.Error(e.Message);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, e.Message); ;
            }
            catch (Exception e) // TODO: Change Exception type
            {
                Log.Error(e.Message);
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
                    Log.Warning("No content found");
                    return NotFound(); // TODO: Edit
                }

                historyMapped.Origin = foundListCurrencyOrigin[0];
                historyMapped.OriginId = foundListCurrencyOrigin[0].Id;
                historyMapped.DestinationId = foundListCurrencyDestination[0].Id;
                historyMapped.Destination = foundListCurrencyDestination[0];
                historyMapped.Result = Utils.Conversion.Convert(historyMapped.Origin, historyMapped.Destination, historyMapped.Value);

                if (history.UserEmail != "") {

                    var foundUsers = await repositoryUser.FindBy(u => u.Email == history.UserEmail).ToListAsync();
                    if (foundUsers.Count == 0)
                    {
                        Log.Warning("No content found");
                        return NotFound(); // TODO: Edit
                    }

                    historyMapped.User = foundUsers[0];
                    historyMapped.UserId = foundUsers[0].Id;
                }

                HistoryForViewDto historyDto = _mapper.Map<HistoryForViewDto>(await repository.Create(historyMapped));
                await repository.SaveDB();

                return Ok(historyDto);
            }
            catch (ArgumentNullException e)
            {
                Log.Error(e.Message);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, e.Message); ;
            }
            catch (OperationCanceledException e)
            {
                Log.Error(e.Message);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, e.Message); ;
            }
            catch (Exception e) // TODO: Change Exception type
            {
                Log.Error(e.Message);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, "Database couldn't be accessed"); ;
            }
        }

    }
}
