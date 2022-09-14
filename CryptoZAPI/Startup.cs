using CryptoZAPI.Models;
using Models;
using Models.Roles;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using Models.Mappers;
using NomixServices;
using Repo;
using RestCountriesServices;
using System.Text;
using Microsoft.IdentityModel.Logging;

public static class Startup {

    public static void ConfigureServices(this WebApplicationBuilder builder) {

        builder.Services
      .AddHttpContextAccessor()
      .AddAuthorization()
      .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
      .AddJwtBearer(options => {
          options.TokenValidationParameters = new TokenValidationParameters {
              ValidateIssuer = true,
              ValidateAudience = true,
              ValidateLifetime = true,
              ValidateIssuerSigningKey = true,
              ValidIssuer = builder.Configuration["Jwt:Issuer"],
              ValidAudience = builder.Configuration["Jwt:Audience"],
              IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
          };
      });


        // AutoMapper
        //builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        builder.Services.AddAutoMapper(typeof(CurrencyProfile), typeof(HistoryProfile), typeof(UserProfile), typeof(CountryProfile));

        // DB Path
        string DbPath = $"DB\\SQLite.DB";

        builder.Services
            .AddSqlite<CryptoZContext>($"Data Source={DbPath}");

        builder.Services.AddIdentityCore<User>(options => {
            options.Password.RequiredLength = 8;
            options.Password.RequireDigit = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
        })
        .AddRoles<UserRole>()
        .AddEntityFrameworkStores<CryptoZContext>();

      


        // Controllers
        builder.Services.AddControllers()
            .AddXmlDataContractSerializerFormatters().
            ConfigureApiBehaviorOptions(setupAction => {
                setupAction.InvalidModelStateResponseFactory = context => {
                    // create a problem details objectk
                    var problemDetailsFactory = context.HttpContext.RequestServices
                        .GetRequiredService<ProblemDetailsFactory>();
                    var problemDetails = problemDetailsFactory.CreateValidationProblemDetails(
                            context.HttpContext,
                            context.ModelState);

                    // add additional info not added by default
                    problemDetails.Detail = "See the errors field for details.";
                    problemDetails.Instance = context.HttpContext.Request.Path;

                    // find out which status code to use
                    var actionExecutingContext =
                        context as Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext;

                    // if there are modelstate errors & all keys were correctly
                    // found/parsed we're dealing with validation errors
                    if ((context.ModelState.ErrorCount > 0) &&
                        (actionExecutingContext?.ActionArguments.Count == context.ActionDescriptor.Parameters.Count)) {
                        problemDetails.Type = "CryptoZ API";
                        problemDetails.Status = StatusCodes.Status422UnprocessableEntity;
                        problemDetails.Title = "One or more validation errors occurred.";

                        return new UnprocessableEntityObjectResult(problemDetails) {
                            ContentTypes = { "application/problem+json" }
                        };
                    }

                    // if one of the keys wasn't correctly found / couldn't be parsed
                    // we're dealing with null/unparsable input
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Title = "One or more errors on input occurred.";
                    return new BadRequestObjectResult(problemDetails) {
                        ContentTypes = { "application/problem+json" }
                    };
                };
            });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Singletons
        builder.Services.AddScoped<INomics, Nomics>();
        builder.Services.AddScoped<IRestCountries, RestCountries>();
        builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        // CORS
        builder.Services.AddCors();



        // Nomics
        builder.Services.AddHttpClient<INomics, Nomics>(client => {
            client.BaseAddress = new Uri("https://api.nomics.com/v1/");
        });

        // RestCountries
        builder.Services.AddHttpClient<IRestCountries, RestCountries>(client => {
            client.BaseAddress = new Uri("https://restcountries.com/v2/");
        });
    }

    public static void Configure(this WebApplication app) {
        if (app.Environment.IsDevelopment()) {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseDeveloperExceptionPage();
            IdentityModelEventSource.ShowPII = true;
        }
        else {
            app.UseExceptionHandler(appBuilder => {
                appBuilder.Run(async context => {
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsync("An unexpected fault happened. Try again later.");
                });
            });

        }

        // Shows UseCors with CorsPolicyBuilder.
        app.UseCors(builder => {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseHttpsRedirection();
        app.MapControllers();
    }
}

