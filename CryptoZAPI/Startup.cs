using CryptoZAPI.Models;
using Models;
using Models.Mappers;
using NomixServices;
using Repo;
using RestCountriesServices;

public static class Startup {

    public static void ConfigureServices(this WebApplicationBuilder builder) {

        // AutoMapper
        //builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        builder.Services.AddAutoMapper(typeof(CurrencyProfile), typeof(HistoryProfile), typeof(UserProfile), typeof(CountryProfile));

        // Controllers
        builder.Services.AddControllers().AddXmlDataContractSerializerFormatters();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Singletons
        builder.Services.AddScoped<INomics, Nomics>();
        builder.Services.AddScoped<IRestCountries, RestCountries>();
        builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

     



        // Nomics
        builder.Services.AddHttpClient<INomics, Nomics>(client => {
            client.BaseAddress = new Uri("https://api.nomics.com/v1/");
        });

        // RestCountries
        builder.Services.AddHttpClient<IRestCountries, RestCountries>(client =>
        {
            client.BaseAddress = new Uri("https://restcountries.com/v2/");
        });
    }

    public static void Configure(this WebApplication app) {
        if (app.Environment.IsDevelopment()) {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsync("An unexpected fault happened. Try again later.");
                });
            });

        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();
    }
}