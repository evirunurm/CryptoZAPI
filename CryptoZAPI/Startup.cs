//public class Startup {

//    public IConfiguration Configuration { get;}

//    public Startup(IConfiguration configuration) {
//        this.Configuration = configuration;
//    }

//    public void ConfigureServices(IServiceCollection services) {
//        services.AddControllers();

//    }

//    public void Configure(IApplicationBuilder app, IHostApplicationLifetime lifetime) {
//    }
//}

using NomixServices;
using Repo;

public static class Startup {

    public static void ConfigureServices(this WebApplicationBuilder builder) {

        // AutoMapper
        builder.Services.AddAutoMapper(typeof(Program));

        // Controllers
        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Singletons
        builder.Services.AddSingleton<INomics, Nomics>();
        builder.Services.AddSingleton<IRepository, Repository>();

        // Nomics
        builder.Services.AddHttpClient<INomics, Nomics>(client => {
            client.BaseAddress = new Uri("https://api.nomics.com/v1/");
        });        
    }

    public static void Configure(this WebApplication app) {
        if (app.Environment.IsDevelopment()) {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();
    }
}