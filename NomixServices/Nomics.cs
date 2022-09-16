using System.Net;
using System;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Models.DTO;

namespace NomixServices {
    public interface INomics {
        Task<List<CurrencyForCreationDto>> getCurrencies();
    }

    public class Nomics : INomics {
        private readonly HttpClient client;
        private readonly IConfiguration configuration;

        public Nomics(HttpClient client, IConfiguration configuration) {
            this.client = client;
            this.configuration = configuration;
        }

        public async Task<List<CurrencyForCreationDto>> getCurrencies() {         
            this.configuration["LastUpdatesAPI:nomics"] = DateTime.Now.Date.ToString();

            string apiKey = configuration["environmentVariables:ApiKey"];

            var result = await client.GetAsync($"currencies/ticker?key={apiKey}");
            var info = await result.Content.ReadAsStringAsync();

            List<CurrencyForCreationDto> currencies = JsonConvert.DeserializeObject<List<CurrencyForCreationDto>>(info) ??
                new List<CurrencyForCreationDto>(); 

            return currencies;
        }
    }
}