using EMS.Users.API.Business.Interfaces.Service;
using EMS.Users.API.Models;
using EMS.Users.API.Models.Dtos;
using EMS.WebAPI.Core.Services;
using FluentValidation.Results;

namespace EMS.Users.API.Business;

public class UserService : MainService, IUserService
{
    private readonly ISubscriberService _subscriberService;
    private readonly IWorkerService _workerService;
    public UserService(ISubscriberService subscriberService, IWorkerService workerService, INotifier notifier) : base(notifier)
    {
        _subscriberService = subscriberService;
        _workerService = workerService;
    }

    public Task<IEnumerable<User>> GetAllUsers()
    {
        throw new NotImplementedException();
    }

    public Task<User> GetByCpf(string cpf)
    {
        throw new NotImplementedException();
    }

    public async Task<ValidationResult> AddUser(UserAddDto user)
    {
        switch (user.UserType)
        {
            case EUserType.Subscriber:
                var subscriber = new Subscriber(user.Id, user.Name, user.Email, user.Cpf);
                await _subscriberService.AddSubscriber(subscriber);
                break;

            case EUserType.Worker:
                var worker = new Worker(user.Id, user.Name, user.Email, user.Cpf, user.SubscriberId ?? Guid.Empty, user.Salary, user.Commission, user.HardSkills);
                await _workerService.AddWorker(worker);
                break;

            default:
                Notify("Falha ao adicionar usuário. Tipo de usuário desconhecido.");
                break;
        }

        return _validationResult;
    }

    public async Task<ValidationResult> UpdateUser(UserUpdDto user)
    {
        switch (user.UserType)
        {
            case EUserType.Subscriber:
                var subscriber = new Subscriber(user.Id, user.Name, user.Email);
                await _subscriberService.UpdateSubscriber(subscriber);
                break;

            case EUserType.Worker:
                var worker = new Worker(user.Id, user.Name, user.Email, user.SubscriberId ?? Guid.Empty, user.Salary, user.Commission, user.HardSkills);
                await _workerService.UpdateWorker(worker);
                break;

            default:
                Notify("Falha ao atualizar usuário. Tipo de usuário desconhecido.");
                break;
        }

        return _validationResult;
    }

    public async Task<ValidationResult> DeleteUser(Guid id, EUserType userType)
    {
        switch (userType)
        {
            case EUserType.Subscriber:
                await _subscriberService.DeleteSubscriber(id);
                break;

            case EUserType.Worker:
                await _workerService.DeleteWorker(id);
                break;

            default:
                Notify("Falha ao deletar usuário. Tipo de usuário desconhecido.");
                break;
        }

        return _validationResult;
    }

}
