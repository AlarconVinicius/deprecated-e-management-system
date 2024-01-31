using EMS.Users.API.Models;
using EMS.Users.API.Models.Dtos;
using FluentValidation.Results;

namespace EMS.Users.API.Business.Interfaces.Service;

public interface IUserService
{
    Task<ValidationResult> AddUser(UserAddDto user);
    Task<ValidationResult> UpdateUser(UserUpdDto user);
    Task<ValidationResult> DeleteUser(Guid id, EUserType userType);
    Task<IEnumerable<User>> GetAllUsers();
    Task<User> GetByCpf(string cpf);
}