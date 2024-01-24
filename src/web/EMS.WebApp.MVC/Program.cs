using EMS.WebApp.MVC.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddIdentityConfiguration();

builder.Services.RegisterServices(builder.Configuration);

builder.Services.AddMvcConfiguration(builder.Configuration);

var app = builder.Build();

app.UseMvcConfiguration(app.Environment);

app.Run();
