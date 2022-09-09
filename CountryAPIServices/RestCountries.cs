using Microsoft.Extensions.Configuration;
using Models.DTO;
using Newtonsoft.Json;

namespace RestCountriesServices {
    public interface IRestCountries {
        Task<List<CountryForCreationDto>> getCountries();
    }

    public class RestCountries : IRestCountries {
        private readonly HttpClient client;
        private readonly IConfiguration configuration;

        public RestCountries(HttpClient client, IConfiguration configuration) {
            this.client = client;
            this.configuration = configuration;
        }

        public async Task<List<CountryForCreationDto>> getCountries() {
            if (DateTime.Parse(this.configuration["LastUpdatesAPI:restCountries"]) >= DateTime.Now.Date) {
                throw new Exception(); // TODO: Not neecsary to update
            }

            this.configuration["LastUpdatesAPI:restCountries"] = DateTime.Now.Date.ToString();

            var result = await client.GetAsync($"all");
            var info = await result.Content.ReadAsStringAsync();

            List<CountryForCreationDto> countries = JsonConvert.DeserializeObject<List<CountryForCreationDto>>(info) ??
                 throw new Exception(); // TODO: Json Excpetion

            return countries;
        }
    }



}