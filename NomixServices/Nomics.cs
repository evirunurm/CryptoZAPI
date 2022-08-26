﻿using System.Net;
using System;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Models.DTO;

namespace NomixServices {
    public interface INomics
    {
        Task<List<CurrencyForViewDto>> getCurrencies();
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

        public async Task<List<CurrencyForViewDto>> getCurrencies()
        {
            string ApiKey = configuration["environmentVariables:ApiKey"];

            var result = await client.GetAsync($"currencies/ticker?key={ApiKey}");
            var info = await result.Content.ReadAsStringAsync();

            // TODO: Check null
            List<CurrencyForViewDto> currencies = JsonConvert.DeserializeObject<List<CurrencyForViewDto>>(info);

            return currencies;
        }
    }
}