using EMS.Core.Data;
using EMS.Users.API.Data;
using EMS.Users.API.Data.Repository;
using EMS.Users.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EMS.Clientes.API.Data.Repository;

public class UserRepository : IUserRepository
{
    private readonly UsersContext _context;

    public UserRepository(UsersContext context)
    {
        _context = context;
    }

    public IUnitOfWork UnitOfWork => _context;

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        return await _context.Users.AsNoTracking().ToListAsync();
    }

    public Task<User> GetByCpf(string cpf)
    {
        return _context.Users.FirstOrDefaultAsync(c => c.Cpf.Number == cpf)!;
    }

    public void AddUser(User user)
    {
        _context.Users.Add(user);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}