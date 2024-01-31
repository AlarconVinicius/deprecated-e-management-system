using EMS.Users.API.Models;
using FluentValidation.Results;

namespace EMS.Users.API.Business.Interfaces.Service;

public interface IClientService
{
    Task<Client> GetByCpf(string cpf, Guid userId);
    Task<IEnumerable<Client>> GetAllClients(Guid userId);
    Task<ValidationResult> AddClient(Client client);
    Task<ValidationResult> UpdateClient(Client client);
    Task<ValidationResult> DeleteClient(Guid id, Guid userId);
}