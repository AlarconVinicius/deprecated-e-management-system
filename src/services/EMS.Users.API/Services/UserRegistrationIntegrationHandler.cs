using EMS.Core.Messages.Integration;
using EMS.MessageBus;
using EMS.Users.API.Business.Interfaces.Service;
using EMS.Users.API.Models;
using EMS.Users.API.Models.Dtos;
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
        var user = new UserAddDto(message.Id, message.Name, message.Email, message.Cpf, 0, 0,"", false, EUserType.Subscriber, null, null);

        using (var scope = _serviceProvider.CreateScope())
        {
            var service = scope.ServiceProvider.GetRequiredService<IUserService>();
            success = await service.AddUser(user);
        }

        return new ResponseMessage(success);
    }
}