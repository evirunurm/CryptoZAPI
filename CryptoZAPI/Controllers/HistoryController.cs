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
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace CryptoZAPI.Controllers {
    [Route("history")]
    [ApiController]
    public class HistoryController : ControllerBase {

        private readonly IRepository<User> repositoryUser;
        private readonly IRepository<Currency> repositoryCurrency;
        private readonly IRepository<History> repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor httpContextAccessor;

        public HistoryController(IRepository<History> repository, IRepository<User> repositoryUser, IRepository<Currency> repositoryCurrency, IMapper mapper,
            IHttpContextAccessor httpContextAccessor) {
            this.repositoryUser = repositoryUser ?? throw new ArgumentNullException(nameof(repositoryUser));
            this.repositoryCurrency = repositoryCurrency;
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        // GET
        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<HistoryForViewDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [Authorize]
        public async Task<IActionResult> GetAll(int userId, int limit = int.MaxValue, int offset = 0) {
            // Get all history where idUser == idUser, with limit limit, ordenador por fecha desc

            try {

                var tokenUserId = AuthController.CheckAuthorizatedUser(httpContextAccessor.HttpContext.User, ClaimTypes.NameIdentifier);

                if (tokenUserId != userId) {
                    return Unauthorized();
                }

                var foundUser = new User(); // await repositoryUser.GetById(userId);

                if (foundUser != null) {
                    Log.Warning("User not found");
                    NotFound();
                }
                IQueryable<History> historyRaw = repository.FindBy(h => h.UserId == userId)
                    .OrderByDescending(h => h.Id);

                List<History> history = await historyRaw
                    .Skip(offset)
                    .Take(limit)
                    .ToListAsync();


                foreach (History h in history) {
                    var foundDestination = await repositoryCurrency.GetById(h.DestinationId);
                    if (foundDestination is null) {
                        Log.Warning("Destination currency not found");
                        return NotFound();
                    }

                    var foundOrigin = await repositoryCurrency.GetById(h.OriginId);
                    if (foundOrigin is null) {
                        Log.Warning("Origin currency not found");
                        return NotFound();
                    }

                    h.Origin = foundOrigin;
                    h.Destination = foundDestination;
                }

                List<HistoryForViewDto> histories;
                histories = _mapper.Map<List<HistoryForViewDto>>(history);

                if (!histories.Any()) {
                    Log.Warning($"No histories found for user with id: {userId}");
                    return NoContent();
                }

                return Ok(histories);

            }
            catch (KeyNotFoundException e) {
                Log.Error(e.Message);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, e.Message); ;
            }
            catch (OperationCanceledException e) {
                Log.Error(e.Message);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, e.Message); ;
            }
            catch (Exception e) // TODO: Change Exception type
            {
                Log.Error(e.Message);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, "Database couldn't be accessed"); ;
            }
        }

        /*
            IDEA
            Hacer 2 POST:
                Uno que sea History/{id o email usuario} -> Guardar la conversion
                Otro que sea History -> No guardar la conversión (para no registrados por ejemplo)
         */
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HistoryForViewDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> Post([FromBody] HistoryForCreationDto history) {
            try {

                if (!ModelState.IsValid) {
                    return new UnprocessableEntityObjectResult(ModelState);
                }

                History historyMapped = _mapper.Map<History>(history);

                var foundListCurrencyOrigin = await repositoryCurrency.FindBy(c => c.Code == history.OriginCode.ToUpper()).ToListAsync();
                var foundListCurrencyDestination = await repositoryCurrency.FindBy(c => c.Code == history.DestinationCode.ToUpper()).ToListAsync();

                if (!foundListCurrencyOrigin.Any() || !foundListCurrencyDestination.Any()) {
                    Log.Warning("No content found");
                    return NotFound(); // TODO: Edit
                }

                historyMapped.Origin = foundListCurrencyOrigin[0];
                historyMapped.OriginId = foundListCurrencyOrigin[0].Id;
                historyMapped.DestinationId = foundListCurrencyDestination[0].Id;
                historyMapped.Destination = foundListCurrencyDestination[0];
                historyMapped.Result = Utils.Conversion.Convert(historyMapped.Origin, historyMapped.Destination, historyMapped.Value);
                historyMapped.Date = DateTime.Now.Date;

                HistoryForViewDto historyDto = _mapper.Map<HistoryForViewDto>(historyMapped);

                return Ok(historyDto);
            }
            catch (KeyNotFoundException e) {
                Log.Error(e.Message);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, e.Message); ;
            }
            catch (OperationCanceledException e) {
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
        [HttpPost("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HistoryForViewDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [Authorize]
        public async Task<IActionResult> Post(int userId, [FromBody] HistoryForCreationDto history) {
            try {
                //[Authorize]

                var tokenUserId = AuthController.CheckAuthorizatedUser(httpContextAccessor.HttpContext.User, ClaimTypes.NameIdentifier);

                if (tokenUserId != userId) {
                    return Unauthorized();
                }

                if (!ModelState.IsValid) {
                    return new UnprocessableEntityObjectResult(ModelState);
                }

                History historyMapped = _mapper.Map<History>(history);

                var foundListCurrencyOrigin = await repositoryCurrency.FindBy(c => c.Code == history.OriginCode.ToUpper()).ToListAsync();
                var foundListCurrencyDestination = await repositoryCurrency.FindBy(c => c.Code == history.DestinationCode.ToUpper()).ToListAsync();
                var foundUser = await repositoryUser.GetById(userId);

                if (!foundListCurrencyOrigin.Any() || !foundListCurrencyDestination.Any()) {
                    Log.Warning("A currency was not found");
                    return NotFound();
                }

                historyMapped.Origin = foundListCurrencyOrigin[0];
                historyMapped.OriginId = foundListCurrencyOrigin[0].Id;
                historyMapped.DestinationId = foundListCurrencyDestination[0].Id;
                historyMapped.Destination = foundListCurrencyDestination[0];
                historyMapped.Result = Utils.Conversion.Convert(historyMapped.Origin, historyMapped.Destination, historyMapped.Value);
                historyMapped.UserId = foundUser.Id;
                historyMapped.Date = DateTime.Now.Date;

                HistoryForViewDto historyDto = _mapper.Map<HistoryForViewDto>(await repository.Create(historyMapped));
                await repository.SaveDB();

                return Ok(historyDto);
            }
            catch (KeyNotFoundException e) {
                Log.Error(e.Message);
                return NotFound();
            }
            catch (OperationCanceledException e) {
                Log.Error(e.Message);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, e.Message);
            }
            catch (Exception e) // TODO: Change Exception type
            {
                Log.Error(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

    }
}
