using EMS.Core.Data;
using EMS.Users.API.Business.Interfaces.Repository;
using EMS.Users.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EMS.Users.API.Data.Repository;

public class ClientRepository : IClientRepository
{
    private readonly UsersContext _context;

    public ClientRepository(UsersContext context)
    {
        _context = context;
    }

    public IUnitOfWork UnitOfWork => _context;

    public async Task<IEnumerable<Client>> GetAllClients()
    {
        return await _context.Clients.AsNoTracking().ToListAsync();
    }

    public async Task<Client> GetById(Guid id)
    {
        return await _context.Clients.FirstOrDefaultAsync(c => c.Id == id) ?? null!;
    }

    public async Task<Client> GetByCpf(string cpf)
    {
        return await _context.Clients.FirstOrDefaultAsync(c => c.Cpf.Number == cpf) ?? null!;
    }

    public void AddClient(Client client)
    {
        _context.Clients.Add(client);
    }

    public void UpdateClient(Client client)
    {
        _context.Clients.Update(client);
    }

    public void DeleteClient(Client client)
    {
        _context.Clients.Remove(client);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}