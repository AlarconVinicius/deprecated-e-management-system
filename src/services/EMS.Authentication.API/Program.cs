using EMS.Authentication.API.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();
builder.Services.AddIdentityConfiguration(builder.Configuration);
builder.Services.AddApiConfiguration();
builder.Services.RegisterServices();
builder.Services.AddSwaggerConfiguration();
//builder.Services.AddMessageBusConfiguration(builder.Configuration);

var app = builder.Build();

app.UseSwaggerConfiguration();

app.UseApiConfiguration(app.Environment);

app.CheckAndApplyDatabaseMigrations(app.Services);

app.Run();
