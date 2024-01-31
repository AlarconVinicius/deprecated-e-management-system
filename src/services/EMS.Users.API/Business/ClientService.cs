using EMS.Users.API.Business.Interfaces.Repository;
using EMS.Users.API.Business.Interfaces.Service;
using EMS.Users.API.Models;
using EMS.WebAPI.Core.Services;
using FluentValidation.Results;

namespace EMS.Users.API.Business;

public class ClientService : MainService, IClientService
{
    private readonly IClientRepository _clientRepository;
    private readonly ISubscriberRepository _subscriberRepository;
    private readonly IWorkerRepository _workerRepository;
    public ClientService(IClientRepository clientRepository, INotifier notifier, IWorkerRepository workerRepository, ISubscriberRepository subscriberRepository) : base(notifier)
    {
        _clientRepository = clientRepository;
        _workerRepository = workerRepository;
        _subscriberRepository = subscriberRepository;
    }

    public async Task<IEnumerable<Client>> GetAllClients(Guid userId)
    {
        var subscriberId = await IsSubscriberOrWorker(userId);

        if(subscriberId == Guid.Empty) return null!;

        return await _clientRepository.GetAllClients(subscriberId);
    }

    public async Task<Client> GetByCpf(string cpf, Guid userId)
    {
        var subscriberId = await IsSubscriberOrWorker(userId);

        if (subscriberId == Guid.Empty) return null!;

        return await _clientRepository.GetByCpf(cpf, subscriberId);
    }

    public async Task<ValidationResult> AddClient(Client client)
    {
        //if (!ExecuteValidation(new ClientValidation(), client)) return _validationResult;

        if (!await SubscriberIdIsValid(client.SubscriberId)) return _validationResult;

        if (await UserExistsByCpf(client.Cpf.Number, client.SubscriberId)) return _validationResult;

        _clientRepository.AddClient(client);

        await PersistData();

        return _validationResult;
    }

    public async Task<ValidationResult> UpdateClient(Client client)
    {
        //if (!ExecuteValidation(new ClientValidation(), subscriber)) return _validationResult;
        var subscriberId = await IsSubscriberOrWorker(client.SubscriberId);

        if (subscriberId == Guid.Empty) return _validationResult;

        if (!await UserExistsById(client.Id, subscriberId)) return _validationResult;

        var clientDb = await _clientRepository.GetById(client.Id, client.SubscriberId);

        clientDb.ChangeName(client.Name);
        clientDb.ChangeEmail(client.Email.Address);

        _clientRepository.UpdateClient(clientDb);

        await PersistData();

        return _validationResult;
    }

    public async Task<ValidationResult> DeleteClient(Guid id, Guid userId)
    {
        var subscriberId = await IsSubscriberOrWorker(userId);

        if (subscriberId == Guid.Empty) return null!;

        var userDb = await _clientRepository.GetById(id, subscriberId);
        if (userDb is null)
        {
            Notify("Usuário não encontrado");
            return _validationResult;
        }
        _clientRepository.DeleteClient(userDb);

        if (!await _clientRepository.UnitOfWork.Commit())
        {
            Notify("Houve um erro ao persistir os dados");
            return _validationResult;
        };
        return _validationResult;
    }


    #region Auxiliary Methods
    private async Task<Guid> IsSubscriberOrWorker(Guid userId)
    {
        Guid searchForId;
        var subsExist = await _subscriberRepository.GetById(userId);
        var workerExist = await _workerRepository.GetById(userId);
        if (subsExist != null!)
        {
            searchForId = subsExist.Id;
        }
        else if (workerExist != null!)
        {
            searchForId = workerExist.SubscriberId;
        }
        else
        {
            Notify("Falha ao buscar clientes.");
            return Guid.Empty;
        };
        if(await SubscriberIdIsValid(searchForId)) return searchForId;

        Notify("Falha ao buscar clientes.");
        return Guid.Empty;
    }
    private async Task<bool> SubscriberIdIsValid(Guid? subscriberId)
    {
        var subscriber = await _subscriberRepository.GetById(subscriberId ?? Guid.Empty);
        if (subscriberId is null || subscriberId == Guid.Empty || subscriber is null)
        {
            Notify("Id do assinante, inválido.");
            return false;
        }
        return true;
    }
    private async Task<bool> UserExistsById(Guid id, Guid userId)
    {
        var subscriberId = await IsSubscriberOrWorker(userId);

        if (subscriberId == Guid.Empty) return false;

        var userExist = await _clientRepository.GetById(id, subscriberId);

        if (userExist is null)
        {
            Notify("Usuário não encontrado.");
            return false;
        };
        return true;
    }

    private async Task<bool> UserExistsByCpf(string cpf, Guid userId)
    {
        var subscriberId = await IsSubscriberOrWorker(userId);

        if (subscriberId == Guid.Empty) return false;

        var userExist = await _clientRepository.GetByCpf(cpf, subscriberId);

        if (userExist != null!)
        {
            Notify("Este CPF já está em uso.");
            return true;
        };
        return false;
    }
    private async Task<ValidationResult> PersistData()
    {
        if (!await _clientRepository.UnitOfWork.Commit())
        {
            Notify("Houve um erro ao persistir os dados");
            return _validationResult;
        };
        return _validationResult;
    }
    #endregion
}
