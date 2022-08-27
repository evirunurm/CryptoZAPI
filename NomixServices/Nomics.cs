using System.Net;
using System;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Models.DTO;

namespace NomixServices {
    public interface INomics
    {
        Task<List<CurrencyForCreationDto>?> getCurrencies();
    }

    public class Nomics : INomics
    {
        private readonly HttpClient client;
        private readonly IConfiguration configuration;
        private int lastRequestMinuteOffset = 60 * 24;

        public Nomics(HttpClient client, IConfiguration configuration)
        {
            this.client = client;
            this.configuration = configuration;
        }

        public async Task<List<CurrencyForCreationDto>?> getCurrencies()
        {   
            // TODO 
            // Solucionar que getCurrencies solo haga una llamada cada X tiempo.
            string apiKey = configuration["environmentVariables:ApiKey"];

            var result = await client.GetAsync($"currencies/ticker?key={apiKey}");
            var info = await result.Content.ReadAsStringAsync();

            List<CurrencyForCreationDto>? currencies = JsonConvert.DeserializeObject<List<CurrencyForCreationDto>>(info) ??
                throw new Exception();
      
            return currencies;
        }
    }
}