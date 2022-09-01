﻿using AutoMapper;
using CryptoZAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Models.DTO;
using Models.Mappers;
using NomixServices;
using Repo;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace CryptoZAPI.Controllers {
    [Route("countries")]
    [ApiController]
    public class CountriesController : ControllerBase {

   
        private readonly IRepository<Country> repository;

        // Mapper
        private readonly IMapper _mapper;



        public CountriesController(IRepository<Country> repository, IMapper mapper) {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        // GET countries
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CurrencyForViewDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> GetAll() {

            await UpdateDatabase(); // This must just update if needed. not return anything.

            //if (actionResultUpdateDb != null) {
            //    return actionResultUpdateDb;
            //}

            try {
                List<CurrencyForViewDto> currencies = _mapper.Map<List<CurrencyForViewDto>>(await repository.GetAll()); // MAPPING FROM Currency TO CurrencyForViewDto 
                
                if (currencies.Count == 0)
                {
                    Log.Warning("No content found");
                    return NoContent();
                }

                return Ok(currencies);
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
            // TODO: Add Exceptions
        }

        // GET currencies/{code}
        [HttpGet("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CurrencyForViewDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> FindOne(string code) {

           await UpdateDatabase();

            //if (actionResultUpdateDb != null) {
            //    return actionResultUpdateDb;
            //}

            try {
                var filtered = await repository.FindBy(c => c.Code == code).ToListAsync(); // Must have only one item

                if (filtered.Count == 0)
                {
                    Log.Warning("Item not found");
                    return NotFound();
                } else if (filtered.Count > 1)
                {
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
            catch (OperationCanceledException e)
            {
                Log.Error(e.Message);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, e.Message); ;
            }

        }

        private async Task UpdateDatabase() {


            try {
                List<CurrencyForCreationDto> NomicsCurrencies = await nomics.getCountries();

                List<Country> CurrenciesToAdd = _mapper.Map<List<Country>>(NomicsCurrencies);


                await repository.CreateRange(CurrenciesToAdd);
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
