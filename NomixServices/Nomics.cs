using CryptoZAPI.Models;
using System.Net;
using System;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace NomixServices
{
    public interface INomics
    {
        Task<List<Currency>> getCurrencies();
    }

    public class Nomics : INomics
    {
        private readonly HttpClient client;
        private readonly IConfiguration configuration;

        public Nomics(HttpClient client, IConfiguration configuration)
        {
            this.client = client;
            this.configuration = configuration;
        }

        public async Task<List<Currency>> getCurrencies()
        {
            string ApiKey = configuration["environmentVariables:ApiKey"];

            Console.WriteLine(ApiKey);
            var result = await client.GetAsync($"currencies/ticker?key={ApiKey}");
            var info = await result.Content.ReadAsStringAsync();

            List<Currency> currencies = JsonConvert.DeserializeObject<List<Currency>>(info);

            return currencies;
        }
    }
}