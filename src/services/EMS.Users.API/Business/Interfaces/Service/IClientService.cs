using EMS.Users.API.Models;
using FluentValidation.Results;

namespace EMS.Users.API.Business.Interfaces.Service;

public interface IClientService
{
    Task<ValidationResult> AddClient(Client client);
    Task<ValidationResult> UpdateClient(Client client);
    Task<ValidationResult> DeleteClient(Guid id);
    Task<IEnumerable<Client>> GetAllClients();
    Task<Client> GetByCpf(string cpf);
}