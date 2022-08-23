using Data;
using Microsoft.EntityFrameworkCore;
using NomixServices;
using Repo;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DB
var connectionString = builder.Configuration.GetConnectionString("AppDb");
builder.Services.AddDbContext<CurrencyContext>(x => x.UseSqlServer(connectionString));

// Custom Services
builder.Services.AddSingleton<INomics, Nomics>();
builder.Services.AddSingleton<IRepository, Repository>();
builder.Services.AddHttpClient<INomics, Nomics>(client => {
    client.BaseAddress = new Uri("https://api.nomics.com/v1/");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
