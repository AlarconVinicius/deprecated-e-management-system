using EMS.Users.API.Business;
using EMS.Users.API.Business.Interfaces.Repository;
using EMS.Users.API.Business.Interfaces.Service;
using EMS.Users.API.Data;
using EMS.Users.API.Data.Repository;
using EMS.WebAPI.Core.Utils;

namespace EMS.Users.API.Configuration;

public static class DependencyInjectionConfig
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<ISubscriberRepository, SubscriberRepository>();
        services.AddScoped<IWorkerRepository, WorkerRepository>();
        services.AddScoped<IClientRepository, ClientRepository>();

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ISubscriberService, SubscriberService>();
        services.AddScoped<IWorkerService, WorkerService>();
        services.AddScoped<IClientService, ClientService>();

        services.AddScoped<UsersContext>();
        services.AddServiceNotifier();
    }
}