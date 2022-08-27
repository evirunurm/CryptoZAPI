using AutoMapper;
using CryptoZAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Models.DTO;
using Models.Mappers;
using NomixServices;
using Repo;

namespace CryptoZAPI.Controllers {
    [Route("currencies")]
    [ApiController]
    public class CurrenciesController : ControllerBase {

        // Logging
        private readonly ILogger<CurrenciesController> _logger;
        private readonly INomics nomics;
        private readonly IRepository repository;

        // Mapper
        private readonly IMapper _mapper;



        public CurrenciesController(ILogger<CurrenciesController> logger, INomics nomics, IRepository repository, IMapper mapper) {
            this._logger = logger;
            this.nomics = nomics;
            this.repository = repository;
            this._mapper = mapper;
        }

        // GET currencies
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CurrencyForViewDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> GetAll() {

            IActionResult? actionResultUpdateDb = await UpdateDatabase();

            if (actionResultUpdateDb != null) {
                return actionResultUpdateDb;
            }

            try {
                List<CurrencyForViewDto> currencies = _mapper.Map<List<CurrencyForViewDto>>(await repository.GetAllCurrencies());
                return Ok(currencies);
            }
            catch (Exception e) // TODO: Change Exception type
            {
                Console.WriteLine(e);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, "Database couldn't be accessed"); ;
            }
        }

        // GET currencies/{code}
        [HttpGet("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CurrencyForViewDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> FindOne(string code) {

            IActionResult? actionResultUpdateDb = await UpdateDatabase();

            if (actionResultUpdateDb != null) {
                return actionResultUpdateDb;
            }

            try {
                CurrencyForViewDto currency = _mapper.Map<CurrencyForViewDto>(await repository.GetOneCurrency(code));
                return Ok(currency);
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                return NotFound();
            }

        }

        private async Task<IActionResult?> UpdateDatabase() {

            IActionResult? actionResult = null;

            try {
                List<CurrencyForCreationDto>? NomicsCurrencies = await nomics.getCurrencies();

                List<Currency> CurrenciesToAdd = _mapper.Map<List<Currency>>(NomicsCurrencies);
                await repository.CreateMultipleCurrencies(CurrenciesToAdd);

                // PROBLEMA (Una de los 2, posiblemente la 2 más que la 1):
                //
                // 1- Como el método es Async, no se cambia el nextRequest en la clase.
                // Solo se cambia en la "instancia" que dura la llamada de está función.
                //
                // 2- El CurrenciesController solo existe durante la llamada del usuario
                // por lo que no tenemos la "referencia" de cuando se ha hecho la ultima
                // request, ya que, cada vez que se hace una llamada a la API, el CurrencyController
                // que recibe dicha llamada es distinto del que había anteriormente.
                //
                // Posible solución:
                // Crear una clase estática y que está se encargue de comprobar el tiempo
            }
            catch (Exception e) // TODO: Change Exception type
            {
                Console.WriteLine($"Excepción {e}");
                actionResult = StatusCode(StatusCodes.Status503ServiceUnavailable, "There's been a problem with our database.");
            }

            return actionResult;
        }
    }
}
