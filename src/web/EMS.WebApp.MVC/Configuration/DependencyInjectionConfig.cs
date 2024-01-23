using EMS.WebAPI.Core.User;
using EMS.WebApp.MVC.Extensions;
using EMS.WebApp.MVC.Services;
using EMS.WebApp.MVC.Services.Handlers;
using Microsoft.AspNetCore.Mvc.DataAnnotations;

namespace EMS.WebApp.MVC.Configuration;

public static class DependencyInjectionConfig
{
    public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IValidationAttributeAdapterProvider, CpfValidationAttributeAdapterProvider>();

        services.AddTransient<HttpClientAuthorizationDelegatingHandler>();

        services.AddHttpClient<IAuthService, AuthService>();

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IAspNetUser, AspNetUser>();

    }
}