using AutoMapper;
using CryptoZAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Models.DTO;
using Models.Mappers;
using NomixServices;
using Repo;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace CryptoZAPI.Controllers {
    [Route("currencies")]
    [ApiController]
    public class CurrenciesController : ControllerBase {

        private readonly INomics nomics;
        private readonly IRepository<Currency> repository;

        // Mapper
        private readonly IMapper _mapper;



        public CurrenciesController(INomics nomics, IRepository<Currency> repository, IMapper mapper) {
            this.nomics = nomics ?? throw new ArgumentNullException(nameof(nomics));
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        // GET currencies
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CurrencyForViewDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> GetAll() {

            await UpdateDatabase();

            try {
                List<CurrencyForViewDto> currencies = _mapper.Map<List<CurrencyForViewDto>>(await repository.GetAll()); // MAPPING FROM Currency TO CurrencyForViewDto 

                if (currencies.Count < 1) {
                    Log.Warning("No content found");
                    return NoContent();
                }

                return Ok(currencies);
            }
            catch (ArgumentNullException e) {
                Console.WriteLine(e);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, e.Message); ;
            }
            catch (OperationCanceledException e) {
                Console.WriteLine(e);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, e.Message); ;
            }
            // TODO: Add Exceptions
        }

        // GET currencies/{id}
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CurrencyForViewDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> FindById(int id) {

            await UpdateDatabase();

            try {
                var foundCurrency = await repository.GetById(id);

                CurrencyForViewDto currency = _mapper.Map<CurrencyForViewDto>(foundCurrency); // MAPPING FROM Currency TO CurrencyForViewDto 

                return Ok(currency);
            }
            catch (ArgumentNullException e) {
                Log.Error(e.Message);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, e.Message); ;
            }
            catch (OperationCanceledException e) {
                Log.Error(e.Message);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, e.Message); ;
            }

        }


        // GET currencies/{code}
        [HttpGet("{code:alpha}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CurrencyForViewDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> FindByCode(string code) {

            await UpdateDatabase();

            try {
                var filtered = await repository.FindBy(c => c.Code == code.ToUpper()).ToListAsync(); // Must have only one item

                if (filtered.Count < 1) {
                    Log.Warning("Item not found");
                    return NotFound();
                }
                else if (filtered.Count > 1) {
                    Log.Warning("Too many items found");
                    // TODO: Not valid code
                }

                CurrencyForViewDto currency = _mapper.Map<CurrencyForViewDto>(filtered[0]); // MAPPING FROM Currency TO CurrencyForViewDto 

                return Ok(currency);
            }
            catch (ArgumentNullException e) {
                Log.Error(e.Message);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, e.Message); ;
            }
            catch (OperationCanceledException e) {
                Log.Error(e.Message);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, e.Message); ;
            }

        }

        private bool Equals(Currency a, Currency b) {
            return a.Code == b.Code || a.Id == b.Id;
        }

        private async Task UpdateDatabase() {


            try {
                List<Currency> NomicsCurrencies = _mapper.Map<List<Currency>>(await nomics.getCurrencies());
                List<Currency> currenciesInContext = (List<Currency>)await repository.GetAll();

                List<Currency> currenciesToAdd = NomicsCurrencies.ExceptBy(currenciesInContext.Select(c => c.Code),
                                                                              x => x.Code).ToList();
                List<Currency> currenciesToUpdate = currenciesInContext.IntersectBy(NomicsCurrencies.Select(c => c.Code),
                                                                              x => x.Code).ToList();
                if (currenciesToAdd.Count > 0) {
                    await repository.CreateRange(currenciesToAdd);
                    await repository.SaveDB();
                }

                // TODO CAMBIAR A UNA LAMBDA O ALGO ASÍ.
                if (currenciesToUpdate.Count > 0) {
                    foreach (Currency currency in currenciesToUpdate) {
                        currency.UpdateFromCurrency(NomicsCurrencies.FirstOrDefault(n => n.Code == currency.Code));
                        repository.Update(currency);
                    }
                    await repository.SaveDB();
                }

            }
            catch (OperationCanceledException e) {
                Log.Warning(e.Message);
                // Didn't update
            }
            catch (Exception e) // TODO: Change Exception type
            {
                Console.WriteLine($"Excepción {e}");
                Log.Error(e.Message);
                // throw Exception

            }

        }
    }
}
