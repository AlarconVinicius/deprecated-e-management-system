using EMS.Users.API.Models;
using FluentValidation.Results;

namespace EMS.Users.API.Business;

public interface IUserService
{
    Task<ValidationResult> AddUser(User user);
    Task<IEnumerable<User>> GetAllUsers();
    Task<User> GetByCpf(string cpf);
}