using Data;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using NomixServices;
using Repo;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

/*
builder.Services.AddMvc()
                .AddMvcOptions(o => o.OutputFormatters.Add(
                   new XmlDataContractSerializerOutputFormatter()))
                         .AddJsonOptions(o =>
                         {
                             if (o.SerializerSettings.ContractResolver != null)
                             {
                                 var castedResolver = o.SerializerSettings.ContractResolver
                                     as DefaultContractResolver;
                                 castedResolver.NamingStrategy = null;
                             }
                         });*/

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Custom Services
builder.Services.AddSingleton<INomics, Nomics>();


//Repository repo = new Repository(_context);
builder.Services.AddSingleton<IRepository, Repository>();


builder.Services.AddHttpClient<INomics, Nomics>(client =>
{
    client.BaseAddress = new Uri("https://api.nomics.com/v1/");
});

var app = builder.Build();

//


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






