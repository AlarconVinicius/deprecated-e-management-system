using EMS.Users.API.Models;
using EMS.Users.API.Models.Dtos;
using FluentValidation.Results;

namespace EMS.Users.API.Business.Interfaces.Service;

public interface IClientService
{
    Task<ClientDto> GetByCpf(string cpf, Guid userId);
    Task<IEnumerable<ClientDto>> GetAllClients(Guid userId);
    Task<ValidationResult> AddClient(Client client);
    Task<ValidationResult> UpdateClient(Client client);
    Task<ValidationResult> DeleteClient(Guid id, Guid userId);
}