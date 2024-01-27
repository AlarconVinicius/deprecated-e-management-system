using EMS.Subscription.API.Business;
using EMS.Subscription.API.Data;
using EMS.Subscription.API.Data.Repository;
using EMS.WebAPI.Core.Utils;

namespace EMS.Subscription.API.Configuration;

public static class DependencyInjectionConfig
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IPlanRepository, PlanRepository>();
        services.AddScoped<IPlanUserRepository, PlanUserRepository>();
        services.AddScoped<IPlanUserService, PlanUserService>();
        services.AddScoped<SubscriptionContext>();
        services.AddServiceNotifier();
    }
}