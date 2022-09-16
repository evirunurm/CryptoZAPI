using AutoMapper;
using CryptoZAPI.Models;
using Microsoft.EntityFrameworkCore;
using Models.DTO;
using NomixServices;
using Quartz;
using Repo;
using RestCountriesServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundTasks {
    public class UpdateCurrencies : IJob {

        private readonly INomics nomics;
        private readonly IRepository<Currency> repositoryCurrency;
        private readonly IMapper _mapper;

        public UpdateCurrencies(INomics nomics, IRepository<Currency> repositoryCurrency, IMapper mapper) {
            this.nomics = nomics ?? throw new ArgumentNullException(nameof(nomics));
            this.repositoryCurrency = repositoryCurrency ?? throw new ArgumentNullException(nameof(repositoryCurrency));
            this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        }

        private async Task UpdateDatabaseCurrencies() {
            Console.WriteLine("UpdatingCurrencies");
            var nomicsReceived = await nomics.getCurrencies();
            List<Currency> NomicsCurrencies = _mapper.Map<List<Currency>>(nomicsReceived);
            List<Currency> currenciesInContext = await repositoryCurrency.GetAll().ToListAsync();

            List<Currency> currenciesToAdd = NomicsCurrencies.ExceptBy(currenciesInContext.Select(c => c.Code),
                                                                          x => x.Code).ToList();

            List<Currency> currenciesToUpdate = currenciesInContext.IntersectBy(NomicsCurrencies.Select(c => c.Code),
                                                                          x => x.Code).ToList();

            if (currenciesToAdd.Any()) {
                await repositoryCurrency.CreateRange(currenciesToAdd);
                await repositoryCurrency.SaveDB();
            }

            if (currenciesToUpdate.Any()) {

                currenciesToUpdate.Select(
                x => {
                    x.UpdateFromCurrency(NomicsCurrencies.FirstOrDefault(c => c.Code == x.Code));
                    return x;
                }).ToList();

                await repositoryCurrency.SaveDB();
            }
        }

        public async Task Execute(IJobExecutionContext context) {
            await UpdateDatabaseCurrencies();
        }
    }
}
