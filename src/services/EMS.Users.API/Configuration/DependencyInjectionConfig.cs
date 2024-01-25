using EMS.Users.API.Data;
using EMS.WebAPI.Core.Utils;

namespace EMS.Users.API.Configuration;

public static class DependencyInjectionConfig
{
    public static void RegisterServices(this IServiceCollection services)
    {
        //services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<UsersContext>();
        services.AddServiceNotifier();
    }
}