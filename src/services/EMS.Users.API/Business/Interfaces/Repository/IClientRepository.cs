using EMS.Core.Data;
using EMS.Users.API.Models;

namespace EMS.Users.API.Business.Interfaces.Repository;

public interface IClientRepository : IRepository<Client>
{
    void AddClient(Client client);
    void UpdateClient(Client client);
    void DeleteClient(Client client);
    Task<IEnumerable<Client>> GetAllClients();
    Task<Client> GetById(Guid id);
    Task<Client> GetByCpf(string cpf);
}
