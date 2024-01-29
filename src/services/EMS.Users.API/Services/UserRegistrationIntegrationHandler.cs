using EMS.Core.Messages.Integration;
using EMS.MessageBus;
using EMS.Users.API.Business;
using EMS.Users.API.Models;
using FluentValidation.Results;

namespace EMS.Users.API.Services;

public class UserRegistrationIntegrationHandler : BackgroundService
{
    private readonly IMessageBus _bus;
    private readonly IServiceProvider _serviceProvider;

    public UserRegistrationIntegrationHandler(
                        IServiceProvider serviceProvider,
                        IMessageBus bus)
    {
        _serviceProvider = serviceProvider;
        _bus = bus;
    }

    private void SetResponder()
    {
        _bus.RespondAsync<RegisteredIdentityIntegrationEvent, ResponseMessage>(async request =>
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

    private async Task<ResponseMessage> RegisterUser(RegisteredIdentityIntegrationEvent message)
    {
        ValidationResult success;
        var user = new User(message.Id, message.Name, message.Email, message.Cpf);

        using (var scope = _serviceProvider.CreateScope())
        {
            var service = scope.ServiceProvider.GetRequiredService<IUserService>();
            success = await service.AddUser(user);
        }

        return new ResponseMessage(success);
    }
}