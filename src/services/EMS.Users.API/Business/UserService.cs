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
    private readonly IClientService _clientService;
    public UserService(ISubscriberService subscriberService, IWorkerService workerService, IClientService clientService, INotifier notifier) : base(notifier)
    {
        _subscriberService = subscriberService;
        _workerService = workerService;
        _clientService = clientService;
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
        if (user.UserType == EUserType.Subscriber)
        {
            var userSub = new Subscriber(user.Id, user.Name, user.Email, user.Cpf);
            await _subscriberService.AddSubscriber(userSub);
            return _validationResult;
        }
        if (user.UserType == EUserType.Worker)
        {
            var userWork = new Worker(user.Id, user.Name, user.Email, user.Cpf, user.SubscriberId ?? Guid.Empty, user.Salary, user.Commission, user.HardSkills);
            await _workerService.AddWorker(userWork);
            return _validationResult;
        }
        if (user.UserType == EUserType.Client)
        {
            var userClient = new Client(user.Id, user.Name, user.Email, user.Cpf, user.SubscriberId ?? Guid.Empty);
            await _clientService.AddClient(userClient);
            return _validationResult;
        }

        Notify("Falha ao adicionar usuário.");
        return _validationResult;
    }

    public async Task<ValidationResult> UpdateUser(UserUpdDto user)
    {
        if (user.UserType == EUserType.Subscriber)
        {
            var userSub = new Subscriber(user.Id, user.Name, user.Email);
            await _subscriberService.UpdateSubscriber(userSub);
            return _validationResult;
        }
        if (user.UserType == EUserType.Worker)
        {
            var userWork = new Worker(user.Id, user.Name, user.Email, user.SubscriberId ?? Guid.Empty, user.Salary, user.Commission, user.HardSkills);
            await _workerService.UpdateWorker(userWork);
            return _validationResult;
        }
        if (user.UserType == EUserType.Client)
        {
            var userClient = new Client(user.Id, user.Name, user.Email, user.SubscriberId ?? Guid.Empty);
            await _clientService.UpdateClient(userClient);
            return _validationResult;
        }

        Notify("Falha ao atualizar usuário.");
        return _validationResult;
    }

    public async Task<ValidationResult> DeleteUser(Guid id, EUserType userType)
    {
        if (userType == EUserType.Subscriber)
        {
            await _subscriberService.DeleteSubscriber(id);
            return _validationResult;
        }
        if (userType == EUserType.Worker)
        {
            await _workerService.DeleteWorker(id);
            return _validationResult;
        }
        if (userType == EUserType.Client)
        {
            await _clientService.DeleteClient(id);
            return _validationResult;
        }
        Notify("Falha ao deletar usuário.");
        return _validationResult;
    }

}
