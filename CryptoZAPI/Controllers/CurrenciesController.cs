using AutoMapper;
using CryptoZAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Models.DTO;
using Models.Mappers;
using NomixServices;
using Repo;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CryptoZAPI.Controllers {
    [Route("currencies")]
    [ApiController]
    public class CurrenciesController : ControllerBase {

        private readonly INomics nomics;
        private readonly IRepository<Currency> repository;
        private readonly IRepository<UserCurrency> repositoryUserCurrency;
        private readonly IHttpContextAccessor httpContextAccessor;

        // Mapper
        private readonly IMapper _mapper;

        public CurrenciesController(INomics nomics, IRepository<Currency> repository,
             IRepository<UserCurrency> repositoryUserCurrency, IMapper mapper, IHttpContextAccessor httpContextAccessor) {
            this.nomics = nomics ?? throw new ArgumentNullException(nameof(nomics));
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.repositoryUserCurrency = repositoryUserCurrency ?? throw new ArgumentNullException(nameof(repositoryUserCurrency));
            this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.httpContextAccessor = httpContextAccessor;
        }

        // GET currencies
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CurrencyForViewDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> GetAll(int limit = int.MaxValue, int offset = 0, string? filter = "") {
            
            try {
                filter = filter ?? "";

                List<CurrencyForViewDto> currencies = _mapper.Map<List<CurrencyForViewDto>>(await repository.GetAll()
                    .Where(currency =>
                        currency.Code.ToUpper().Contains(filter.ToUpper()) ||
                        currency.Name.ToUpper().Contains(filter.ToUpper())
                    )
                    .Skip(offset)
                    .Take(limit)
                    .ToListAsync()
                ); // MAPPING FROM Currency TO CurrencyForViewDto 

                if (currencies.Count < 1) {
                    Log.Warning("No content found");
                    return NoContent();
                }

                return Ok(currencies);
            }
            catch (KeyNotFoundException e) {
                Console.WriteLine(e);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, e.Message);
            }
            catch (OperationCanceledException e) {
                Console.WriteLine(e);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, e.Message);
            }
            catch (Exception e) {
                Console.WriteLine(e);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, e.Message);
            }
            // TODO: Add Exceptions
        }


        // GET currencies/{id}
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CurrencyForViewDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> FindById(int id) {
            try {
                var foundCurrency = await repository.GetById(id);

                CurrencyForViewDto currency = _mapper.Map<CurrencyForViewDto>(foundCurrency); // MAPPING FROM Currency TO CurrencyForViewDto 

                return Ok(currency);
            }
            catch (ArgumentNullException e) {
                Log.Error(e.Message);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, e.Message);
            }
            catch (OperationCanceledException e) {
                Log.Error(e.Message);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, e.Message);
            }

        }


        // GET currencies/{code}
        [HttpGet("{code:alpha}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CurrencyForViewDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> FindByCode(string code) {
            try {
                var filtered = await repository.FindBy(c => c.Code == code.ToUpper()).ToListAsync(); // Must have only one item

                if (!filtered.Any()) {
                    Log.Warning("Item not found");
                    return NotFound();
                }

                CurrencyForViewDto currency = _mapper.Map<CurrencyForViewDto>(filtered[0]); // MAPPING FROM Currency TO CurrencyForViewDto 

                return Ok(currency);
            }
            catch (KeyNotFoundException e) {
                Log.Error(e.Message);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, e.Message);
            }
            catch (OperationCanceledException e) {
                Log.Error(e.Message);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, e.Message);
            }

        }

        /* -- CUSTOM CURRENCIES -- */
        // GET currencies
        [HttpGet("user/{userId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CurrencyForViewDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [Authorize]
        public async Task<IActionResult> GetAll(int userId, int limit = int.MaxValue, int offset = 0, string? filter = "") {
            try {
                var tokenUserId = AuthController.CheckAuthorizatedUser(httpContextAccessor.HttpContext.User, ClaimTypes.NameIdentifier);

                if (tokenUserId != userId) {
                    return Unauthorized();
                }

                filter = filter ?? "";

                List<Currency> currencies = (await repository.GetAll()
                    .Where(currency =>
                        currency.Code.ToUpper().Contains(filter.ToUpper()) ||
                        currency.Name.ToUpper().Contains(filter.ToUpper())
                    )
                    .Skip(offset)
                    .Take(limit)
                    .ToListAsync()
                ); // MAPPING FROM Currency TO CurrencyForViewDto 

                if (!currencies.Any()) {
                    Log.Warning("No content found");
                    return NoContent();
                }

                List<UserCurrency> userCurrenciesList = await repositoryUserCurrency.FindBy(c => c.UserId == userId).ToListAsync(); // Must have only one item

                if (userCurrenciesList.Any()) {
                    currencies.Select(
                    x => {
                        UserCurrency? customCurrency = userCurrenciesList.FirstOrDefault(c => c.CurrencyId == x.Id);
                        if (customCurrency == null)
                            return x;
                        x.Name = customCurrency.Name;
                        return x;
                    }).ToList();
                }

                return Ok(currencies);
            }
            catch (KeyNotFoundException e) {
                Console.WriteLine(e);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, e.Message);
            }
            catch (OperationCanceledException e) {
                Console.WriteLine(e);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, e.Message);
            }
            catch (Exception e) {
                Console.WriteLine(e);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, e.Message);
            }
            // TODO: Add Exceptions
        }     

        private bool Equals(Currency a, Currency b) {
            return a.Code == b.Code || a.Id == b.Id;
        }      
    }
}
