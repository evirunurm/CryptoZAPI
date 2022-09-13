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

app.MapPost("/token", async (AuthenticateRequest request, UserManager<User> userManager) => {
    // Verificamos credenciales con Identity
    var user = await userManager.FindByNameAsync(request.UserEmail);

    if (user is null || !await userManager.CheckPasswordAsync(user, request.Password)) {
        return Results.Forbid();
    }

    var roles = await userManager.GetRolesAsync(user);

    // Generamos un token según los claims
    var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Email, user.Email),
    };

    foreach (var role in roles) {
        claims.Add(new Claim(ClaimTypes.Role, role));
    }

    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]));
    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
    var tokenDescriptor = new JwtSecurityToken(
        issuer: builder.Configuration["Jwt:Issuer"],
        audience: builder.Configuration["Jwt:Audience"],
        claims: claims,
        expires: DateTime.Now.AddMinutes(720),
        signingCredentials: credentials);

    var jwt = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

    return Results.Ok(new {
        AccessToken = jwt
    });
});

app.MapGet("/me", (IHttpContextAccessor contextAccessor) => {
    var user = contextAccessor.HttpContext.User;

    return Results.Ok(new {
        Claims = user.Claims.Select(s => new {
            s.Type,
            s.Value
        }).ToList(),
        user.Identity.Name,
        user.Identity.IsAuthenticated,
        user.Identity.AuthenticationType
    });
})
.RequireAuthorization();

await SeedData();

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
        Name = "Test",
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

