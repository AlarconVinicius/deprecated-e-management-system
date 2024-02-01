using EMS.Users.API.Business.Interfaces.Repository;
using EMS.Users.API.Business.Interfaces.Service;
using EMS.Users.API.Models;
using EMS.WebAPI.Core.Services;
using FluentValidation.Results;

namespace EMS.Users.API.Business;

public class SubscriberService : MainService, ISubscriberService
{
    private readonly ISubscriberRepository _subscriberRepository;
    public SubscriberService(ISubscriberRepository subscriberRepository, INotifier notifier) : base(notifier)
    {
        _subscriberRepository = subscriberRepository;
    }

    public Task<IEnumerable<Subscriber>> GetAllSubscribers()
    {
        throw new NotImplementedException();
    }

    public Task<Subscriber> GetByCpf(string cpf)
    {
        throw new NotImplementedException();
    }
    public async Task<ValidationResult> AddSubscriber(Subscriber subscriber)
    {
        //if (!ExecuteValidation(new UserValidation(), subscriber)) return _validationResult;

        if (await UserExists(subscriber.Cpf.Number)) return _validationResult;

        _subscriberRepository.AddSubscriber(subscriber);

        await PersistData();

        return _validationResult;
    }

    public async Task<ValidationResult> UpdateSubscriber(Subscriber subscriber)
    {
        //if (!ExecuteValidation(new UserValidation(), subscriber)) return _validationResult;

        var subscriberDb = await _subscriberRepository.GetById(subscriber.Id);

        subscriberDb.ChangeName(subscriber.Name);
        subscriberDb.ChangeEmail(subscriber.Email.Address);

        _subscriberRepository.UpdateSubscriber(subscriberDb);

        await PersistData();

        return _validationResult;
    }

    public async Task<ValidationResult> DeleteSubscriber(Guid id)
    {
        var userDb = await _subscriberRepository.GetById(id);
        if (userDb is null)
        {
            Notify("Usuário não encontrado");
            return _validationResult;
        }
        if(!await _subscriberRepository.DeleteSubscriber(userDb))
        {
            Notify("Houve um erro ao deletar usuário");
            return _validationResult;
        };

        if (!await _subscriberRepository.UnitOfWork.Commit())
        {
            Notify("Houve um erro ao persistir os dados");
            return _validationResult;
        };
        return _validationResult;
    }

    #region Auxiliary Methods
    private async Task<bool> UserExists(string cpf)
    {
        var userExist = await _subscriberRepository.GetByCpf(cpf);

        if (userExist != null!)
        {
            Notify("Este CPF já está em uso.");
            return true;
        };
        return false;
    }
    private async Task<ValidationResult> PersistData()
    {
        if (!await _subscriberRepository.UnitOfWork.Commit())
        {
            Notify("Houve um erro ao persistir os dados");
            return _validationResult;
        };
        return _validationResult;
    }
    #endregion

}
