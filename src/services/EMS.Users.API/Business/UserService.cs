using EMS.Users.API.Data.Repository;
using EMS.Users.API.Models;
using EMS.Users.API.Models.Validations;
using EMS.WebAPI.Core.Services;
using FluentValidation.Results;

namespace EMS.Users.API.Business;

public class UserService : MainService, IUserService
{
    private readonly IUserRepository _userRepository;
    public UserService(IUserRepository userRepository, INotifier notifier) : base(notifier)
    {
        _userRepository = userRepository;
    }

    public async Task<ValidationResult> AddUser(User user)
    {
        if (!ExecuteValidation(new UserValidation(), user)) return _validationResult;

        var userExist = await _userRepository.GetByCpf(user.Cpf.Number);

        if (userExist != null!)
        {
            Notify("Este CPF já está em uso.");
            return _validationResult;
        }
        _userRepository.AddUser(user);

        if(!await _userRepository.UnitOfWork.Commit())
        {
            Notify("Houve um erro ao persistir os dados");
            return _validationResult;
        };
        return _validationResult;
    }

    public async Task<ValidationResult> DeleteUser(string cpf)
    {
        var userDb = await _userRepository.GetByCpf(cpf);
        if (userDb is null)
        {
            Notify("Usuário não encontrado");
            return _validationResult;
        }
        _userRepository.DeleteUser(userDb);

        if (!await _userRepository.UnitOfWork.Commit())
        {
            Notify("Houve um erro ao persistir os dados");
            return _validationResult;
        };
        return _validationResult;
    }

    public Task<IEnumerable<User>> GetAllUsers()
    {
        throw new NotImplementedException();
    }

    public Task<User> GetByCpf(string cpf)
    {
        throw new NotImplementedException();
    }
}
