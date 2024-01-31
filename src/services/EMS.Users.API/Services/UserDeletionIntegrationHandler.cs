using EMS.Core.Messages.Integration;
using EMS.MessageBus;
using EMS.Users.API.Business.Interfaces.Service;
using EMS.Users.API.Models;
using FluentValidation.Results;

namespace EMS.Users.API.Services;

public class UserDeletionIntegrationHandler : BackgroundService
{
    private readonly IMessageBus _bus;
    private readonly IServiceProvider _serviceProvider;

    public UserDeletionIntegrationHandler(
                        IServiceProvider serviceProvider,
                        IMessageBus bus)
    {
        _serviceProvider = serviceProvider;
        _bus = bus;
    }

    private void SetResponder()
    {
        _bus.RespondAsync<DeletedUserIntegrationEvent, ResponseMessage>(async request =>
            await DeleteUser(request));

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

    private async Task<ResponseMessage> DeleteUser(DeletedUserIntegrationEvent message)
    {
        ValidationResult success;
        using (var scope = _serviceProvider.CreateScope())
        {
            var service = scope.ServiceProvider.GetRequiredService<IUserService>();
            success = await service.DeleteUser(message.Id, EUserType.Subscriber);
        }

        return new ResponseMessage(success);
    }
}