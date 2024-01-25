using EMS.Users.API.Business;
using EMS.Users.API.Data;
using EMS.Users.API.Data.Repository;
using EMS.WebAPI.Core.Utils;

namespace EMS.Users.API.Configuration;

public static class DependencyInjectionConfig
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<UsersContext>();
        services.AddServiceNotifier();
    }
}