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
    public class UpdateCountries : IJob {

        private readonly IRestCountries restCountries;
        private readonly IRepository<Country> repositoryCountry;
        private readonly IMapper _mapper;

        public UpdateCountries(IRestCountries restCountries,
            IRepository<Country> repositoryCountry, IMapper mapper) {
            this.restCountries = restCountries ?? throw new ArgumentNullException(nameof(restCountries));
            this.repositoryCountry = repositoryCountry ?? throw new ArgumentNullException(nameof(repositoryCountry));
            this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        }

        private async Task UpdateDatabaseCountries() {
            Console.WriteLine("UpdatingCountries");

            List<Country> CurrentCountry = _mapper.Map<List<Country>>(await restCountries.getCountries());

            List<Country> countriesInContext = await repositoryCountry.GetAll().ToListAsync();

            List<Country> countriesToAdd = CurrentCountry.ExceptBy(countriesInContext.Select(c => c.CountryCode),
                                                                          x => x.CountryCode).ToList();

            List<Country> countriesToUpdate = countriesInContext.IntersectBy(CurrentCountry.Select(c => c.CountryCode),
                                                                          x => x.CountryCode).ToList();

            if (countriesToAdd.Any()) {
                await repositoryCountry.CreateRange(countriesToAdd);
                await repositoryCountry.SaveDB();
            }

            if (countriesToUpdate.Any()) {

                countriesToUpdate.Select(
                x => {
                    x.UpdateFromCountry(CurrentCountry.FirstOrDefault(c => c.CountryCode == x.CountryCode));
                    return x;
                }).ToList();

                await repositoryCountry.SaveDB();
            }
        }

        public async Task Execute(IJobExecutionContext context) {
            await UpdateDatabaseCountries();
        }
    }
}
