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
        public async Task<IActionResult> GetAll(int limit = int.MaxValue, int offset = 0, string? filter = "" ) {

            await UpdateDatabase();

            filter = filter ?? "";

            //if (filter is null)
            //{
            //    filter = "";
            //}

            try {
                

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
            catch (Exception e)
            {
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

            await UpdateDatabase();

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

            await UpdateDatabase();

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

        private bool Equals(Currency a, Currency b) {
            return a.Code == b.Code || a.Id == b.Id;
        }
        private async Task UpdateDatabase() {
            try {
                List<Currency> NomicsCurrencies = _mapper.Map<List<Currency>>(await nomics.getCurrencies());
                List<Currency> currenciesInContext = await repository.GetAll().ToListAsync();

                List<Currency> currenciesToAdd = NomicsCurrencies.ExceptBy(currenciesInContext.Select(c => c.Code),
                                                                              x => x.Code).ToList();

                List<Currency> currenciesToUpdate = currenciesInContext.IntersectBy(NomicsCurrencies.Select(c => c.Code),
                                                                              x => x.Code).ToList();

                if (currenciesToAdd.Any()) {
                    await repository.CreateRange(currenciesToAdd);
                    await repository.SaveDB();
                }

                if (currenciesToUpdate.Any()) {

                    currenciesToUpdate.Select(
                    x => {
                        x.UpdateFromCurrency(NomicsCurrencies.FirstOrDefault(c => c.Code == x.Code));
                        return x;
                    }).ToList();

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
