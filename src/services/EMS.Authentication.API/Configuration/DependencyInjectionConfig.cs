using EMS.Authentication.API.Business;
using EMS.WebAPI.Core.Utils;

namespace EMS.Authentication.API.Configuration;

public static class DependencyInjectionConfig
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddServiceNotifier();
    }
}