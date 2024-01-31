using EMS.Users.API.Business.Interfaces.Repository;
using EMS.Users.API.Data.Repository;
using EMS.Users.API.Models;
using EMS.Users.API.Models.Dtos;
using EMS.WebAPI.Core.Services;
using FluentValidation.Results;

namespace EMS.Users.API.Business;

public class ClientService : MainService, IClientService
{
    private readonly IClientRepository _clientRepository;
    public ClientService(IClientRepository clientRepository, INotifier notifier) : base(notifier)
    {
        _clientRepository = clientRepository;
    }

    public Task<IEnumerable<Client>> GetAllClients()
    {
        throw new NotImplementedException();
    }

    public Task<Client> GetByCpf(string cpf)
    {
        throw new NotImplementedException();
    }

    public async Task<ValidationResult> AddClient(Client client)
    {
        //if (!ExecuteValidation(new ClientValidation(), client)) return _validationResult;

        if (!SubscriberIdIsValid(client.SubscriberId)) return _validationResult;

        if (await UserExists(client.Cpf.Number)) return _validationResult;

        _clientRepository.AddClient(client);

        await PersistData();

        return _validationResult;
    }

    public async Task<ValidationResult> UpdateClient(Client client)
    {
        //if (!ExecuteValidation(new ClientValidation(), subscriber)) return _validationResult;

        var clientDb = await _clientRepository.GetById(client.Id);

        clientDb.ChangeName(client.Name);
        clientDb.ChangeEmail(client.Email.Address);

        _clientRepository.UpdateClient(clientDb);

        await PersistData();

        return _validationResult;
    }

    public async Task<ValidationResult> DeleteClient(Guid id)
    {
        var userDb = await _clientRepository.GetById(id);
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
    private async Task<bool> UserExists(string cpf)
    {
        var userExist = await _clientRepository.GetByCpf(cpf);

        if (userExist != null!)
        {
            Notify("Este CPF já está em uso.");
            return true;
        };
        return false;
    }
    private bool SubscriberIdIsValid(Guid? subscriberId)
    {
        if (subscriberId is null || subscriberId == Guid.Empty)
        {
            Notify("Id do assinante, inválido.");
            return false;
        }
        return true;
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
