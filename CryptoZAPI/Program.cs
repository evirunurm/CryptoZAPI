using Data;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using NomixServices;
using Repo;
using Serilog;
using static System.Net.Mime.MediaTypeNames;


var builder = WebApplication.CreateBuilder(args);

builder.ConfigureServices();

Log.Logger = new LoggerConfiguration()
    .WriteTo.File("logs.log")
    .CreateLogger();

Log.Information("Starting App");

var app = builder.Build();

app.Configure();

app.Run();



