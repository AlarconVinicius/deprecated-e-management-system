using EMS.Core.Data;
using EMS.Users.API.Models;

namespace EMS.Users.API.Data.Repository;

public interface IUserRepository : IRepository<User>
{
    void AddUser(User user);
    void DeleteUser(User user);
    Task<IEnumerable<User>> GetAllUsers();
    Task<User> GetByCpf(string cpf);
}
