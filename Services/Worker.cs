using AutoMapper;
using CryptoZAPI.Models;
using Microsoft.EntityFrameworkCore;
using Models.DTO;
using NomixServices;
using Repo;
using RestCountriesServices;
using Serilog;


namespace Services {
    public class AutoUpdate : BackgroundService {
        private readonly ILogger<AutoUpdate> _logger;
        private readonly INomics nomics;
        private readonly IRestCountries restCountries;

        private readonly IRepository<Country> repositoryCountry;
        private readonly IRepository<Currency> repositoryCurrency;

        private readonly IMapper _mapper;

        //public AutoUpdate(ILogger<AutoUpdate> _logger) {
        //    this._logger = _logger; 

        //}
        public AutoUpdate(ILogger<AutoUpdate> logger, INomics nomics, IRestCountries restCountries,
            IRepository<Country> repositoryCountry, IRepository<Currency> repositoryCurrency, IMapper mapper) {
            _logger = logger;
            this.nomics = nomics;
            this.restCountries = restCountries;
            this.repositoryCountry = repositoryCountry;
            this.repositoryCurrency = repositoryCurrency;
            _mapper = mapper;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            _logger.LogDebug("Starting");

            stoppingToken.Register(() =>
                _logger.LogDebug("Stopping."));

            while (!stoppingToken.IsCancellationRequested) {
                _logger.LogDebug($"Working");

                // Your code here
                Console.WriteLine("Updating DB");
                await UpdateDatabaseCountries();
                await UpdateDatabaseCurrencies();
                //

                await Task.Delay(TimeSpan.FromMinutes(100), stoppingToken);
            }
            _logger.LogDebug($"Stopping.");
        }

        private async Task UpdateDatabaseCountries() {

            //try {
            //    List<CountryForCreationDto> CurrentCountry = await restCountries.getCountries();
            //    List<Country> CountriesToAdd = _mapper.Map<List<Country>>(CurrentCountry);

            //    await repositoryCountry.CreateRange(CountriesToAdd);
            //    await repositoryCountry.SaveDB();
            //}
            //catch (OperationCanceledException e) {
            //    Log.Warning(e.Message);
            //    // Didn't update
            //}
            //catch (Exception e) // TODO: Change Exception type
            //{
            //    Console.WriteLine($"Excepción {e}");
            //    Log.Error(e.Message);
            //    // throw Exception

            //}
        }
        private async Task UpdateDatabaseCurrencies() {
            //try {
            //    List<Currency> NomicsCurrencies = _mapper.Map<List<Currency>>(await nomics.getCurrencies());
            //    List<Currency> currenciesInContext = await repositoryCurrency.GetAll().ToListAsync();

            //    List<Currency> currenciesToAdd = NomicsCurrencies.ExceptBy(currenciesInContext.Select(c => c.Code),
            //                                                                  x => x.Code).ToList();

            //    List<Currency> currenciesToUpdate = currenciesInContext.IntersectBy(NomicsCurrencies.Select(c => c.Code),
            //                                                                  x => x.Code).ToList();

            //    if (currenciesToAdd.Any()) {
            //        await repositoryCurrency.CreateRange(currenciesToAdd);
            //        await repositoryCurrency.SaveDB();
            //    }

            //    if (currenciesToUpdate.Any()) {

            //        currenciesToUpdate.Select(
            //        x => {
            //            x.UpdateFromCurrency(NomicsCurrencies.FirstOrDefault(c => c.Code == x.Code));
            //            return x;
            //        }).ToList();

            //        await repositoryCurrency.SaveDB();
            //    }

            //}
            //catch (OperationCanceledException e) {
            //    Log.Warning(e.Message);
            //    // Didn't update
            //}
            //catch (Exception e) // TODO: Change Exception type
            //{
            //    Console.WriteLine($"Excepción {e}");
            //    Log.Error(e.Message);
            //    // throw Exception
            //}
        }
    }
}