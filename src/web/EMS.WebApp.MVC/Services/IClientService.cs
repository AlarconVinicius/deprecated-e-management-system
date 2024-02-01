using EMS.WebApp.MVC.Models;

namespace EMS.WebApp.MVC.Services;

public interface IClientService
{
    Task<IEnumerable<ClientViewModel>> GetAll(Guid userId);
    Task<ClientViewModel> GetByCpf(string cpf, Guid userId);
    Task<ClientResponse> AddClient(ClientViewModel client);
    Task<ClientResponse> UpdateClient(ClientViewModel client);
    Task<ClientResponse> DeleteClient(Guid id, Guid userId);
}