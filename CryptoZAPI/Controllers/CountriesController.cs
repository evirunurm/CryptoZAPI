using AutoMapper;
using CryptoZAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Models.DTO;
using Models.Mappers;
using Repo;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Models;
using RestCountriesServices;

namespace CryptoZAPI.Controllers {
    [Route("countries")]
    [ApiController]
    public class CountriesController : ControllerBase {


        private readonly IRestCountries restCountries;
        private readonly IRepository<Country> repository;

        // Mapper
        private readonly IMapper _mapper;
        



        public CountriesController(IRestCountries restCountries, IRepository<Country> repository, IMapper mapper) {
            this.restCountries = restCountries ?? throw new ArgumentNullException(nameof(restCountries));
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
     
        }

        // GET countries
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CountryForViewDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> GetAll() {
            
            
            await UpdateDatabase(); // This must just update if needed. not return anything.           

            try {
                List<CountryForViewDto> countries = _mapper.Map<List<CountryForViewDto>>(await repository.GetAll()); // MAPPING FROM Currency TO CurrencyForViewDto 
                
                if (countries.Count == 0)
                {
                    Log.Warning("No content found");
                    return NoContent();
                }

                return Ok(countries);
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
        }

        // GET countries/{countrycode}
        [HttpGet("{countrycode:alpha}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CountryForViewDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> FindByCode(string countrycode) {
            
           await UpdateDatabase();      

            try {
                var filtered = await repository.FindBy(c => c.CountryCode == countrycode.ToUpper()).ToListAsync(); // Must have only one item

                if (filtered.Count == 0)
                {
                    Log.Warning("Item not found");
                    return NotFound();
                } else if (filtered.Count > 1)
                {
                    Log.Warning("Too many items found");
                    // TODO: Not valid code
                }

                CountryForViewDto country = _mapper.Map<CountryForViewDto>(filtered[0]); // MAPPING FROM Currency TO CurrencyForViewDto 
                
                return Ok(country);
            }
            catch (KeyNotFoundException e) {
                Log.Error(e.Message);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, e.Message); ;
            }
            catch (OperationCanceledException e)
            {
                Log.Error(e.Message);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, e.Message); ;
            }
        }


        // GET countries/{id}
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CountryForViewDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> FindById(int id)
        {

            await UpdateDatabase();

            try
            {
                var foundCountry = await repository.GetById(id);

                CountryForViewDto country = _mapper.Map<CountryForViewDto>(foundCountry); // MAPPING FROM Currency TO CurrencyForViewDto 

                return Ok(country);
            }
            catch (KeyNotFoundException e)
            {
                Log.Error(e.Message);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, e.Message); ;
            }
            catch (OperationCanceledException e)
            {
                Log.Error(e.Message);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, e.Message); ;
            }
        }


        private async Task UpdateDatabase() {
            
            try {
                List<CountryForCreationDto> CurrentCountry = await restCountries.getCountries();

                List<Country> CountriesToAdd = _mapper.Map<List<Country>>(CurrentCountry);


                await repository.CreateRange(CountriesToAdd);
                await repository.SaveDB();
            }
            catch (OperationCanceledException e)
            {
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
