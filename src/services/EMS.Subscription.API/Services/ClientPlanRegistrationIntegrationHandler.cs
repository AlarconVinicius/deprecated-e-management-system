using EMS.Core.Messages.Integration;
using EMS.MessageBus;
using EMS.Subscription.API.Business;
using EMS.Subscription.API.Model;
using EMS.Subscription.API.Models;
using FluentValidation.Results;

namespace EMS.Subscription.API.Services;

public class ClientPlanRegistrationIntegrationHandler : BackgroundService
{
    private readonly IMessageBus _bus;
    private readonly IServiceProvider _serviceProvider;

    public ClientPlanRegistrationIntegrationHandler(
                        IServiceProvider serviceProvider,
                        IMessageBus bus)
    {
        _serviceProvider = serviceProvider;
        _bus = bus;
    }

    private void SetResponder()
    {
        _bus.RespondAsync<RegisteredUserIntegrationEvent, ResponseMessage>(async request =>
            await RegisterUser(request));

        _bus.AdvancedBus.Connected += OnConnect!;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        SetResponder();
        return Task.CompletedTask;
    }

    private void OnConnect(object s, EventArgs e)
    {
        SetResponder();
    }

    private async Task<ResponseMessage> RegisterUser(RegisteredUserIntegrationEvent message)
    {
        ValidationResult success;
        var planUser = new PlanUser(message.PlanId, message.Id, message.Name, message.Email, message.Cpf, message.IsActive);

        using (var scope = _serviceProvider.CreateScope())
        {
            var service = scope.ServiceProvider.GetRequiredService<IPlanUserService>();
            success = await service.AddPlanUser(planUser);
        }

        return new ResponseMessage(success);
    }
}