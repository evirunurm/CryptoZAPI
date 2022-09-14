using Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using NomixServices;
using Repo;
using Serilog;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Identity;
using static CryptoZAPI.Controllers.UsersController;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Models.Roles;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureServices();

Log.Logger = new LoggerConfiguration()
    .WriteTo.File("logs.log")
    .CreateLogger();

Log.Information("Starting App");

var app = builder.Build();

app.Configure();

// await SeedData();

app.Run();



async Task SeedData() {
    var scopeFactory = app!.Services.GetRequiredService<IServiceScopeFactory>();
    using var scope = scopeFactory.CreateScope();

    var context = scope.ServiceProvider.GetRequiredService<CryptoZContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<UserRole>>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    context.Database.EnsureCreated();

    //if (!userManager.Users.Any()) {
    logger.LogInformation("Creando usuario de prueba");

    var newUser = new User {
        Email = "test@demo.com",
        FullName = "Test",
        UserName = "patata",
        CountryId = 1
    };

    await userManager.CreateAsync(newUser, "P@ss.W0rd");
    //await roleManager.CreateAsync(new UserRole {
    //    Notes = "Admin"
    //});
    //await roleManager.CreateAsync(new UserRole {
    //    Notes = "AnotherRole"
    //});

    bool x = await roleManager.RoleExistsAsync("Admin");
    if (!x) {
        // first we create Admin role   
        var role = new UserRole();
        role.Name = "Admin";
        role.Notes = "test";
        await roleManager.CreateAsync(role);
    }

    await userManager.AddToRoleAsync(newUser, "Admin");
    //}
}

