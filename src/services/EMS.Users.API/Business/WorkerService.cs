using EMS.Users.API.Business.Interfaces.Repository;
using EMS.Users.API.Business.Interfaces.Service;
using EMS.Users.API.Models;
using EMS.WebAPI.Core.Services;
using FluentValidation.Results;

namespace EMS.Users.API.Business;

public class WorkerService : MainService, IWorkerService
{
    private readonly IWorkerRepository _workerRepository;
    public WorkerService(IWorkerRepository workerRepository, INotifier notifier) : base(notifier)
    {
        _workerRepository = workerRepository;
    }

    public Task<IEnumerable<Worker>> GetAllWorkers()
    {
        throw new NotImplementedException();
    }

    public Task<Worker> GetByCpf(string cpf)
    {
        throw new NotImplementedException();
    }

    public async Task<ValidationResult> AddWorker(Worker worker)
    {
        //if (!ExecuteValidation(new WorkerValidation(), worker)) return _validationResult;

        if (!SubscriberIdIsValid(worker.SubscriberId)) return _validationResult;

        if (await UserExists(worker.Cpf.Number)) return _validationResult;

        _workerRepository.AddWorker(worker);

        await PersistData();

        return _validationResult;
    }

    public async Task<ValidationResult> UpdateWorker(Worker worker)
    {
        //if (!ExecuteValidation(new WorkerValidation(), subscriber)) return _validationResult;

        var workerDb = await _workerRepository.GetById(worker.Id);

        workerDb.ChangeName(worker.Name);
        workerDb.ChangeEmail(worker.Email.Address);
        workerDb.ChangeSalary(worker.Salary);
        workerDb.ChangeCommission(worker.Commission);
        workerDb.ChangeHardSkills(worker.HardSkills);

        _workerRepository.UpdateWorker(workerDb);

        await PersistData();

        return _validationResult;
    }

    public async Task<ValidationResult> DeleteWorker(Guid id)
    {
        var userDb = await _workerRepository.GetById(id);
        if (userDb is null)
        {
            Notify("Usuário não encontrado");
            return _validationResult;
        }
        _workerRepository.DeleteWorker(userDb);

        if (!await _workerRepository.UnitOfWork.Commit())
        {
            Notify("Houve um erro ao persistir os dados");
            return _validationResult;
        };
        return _validationResult;
    }


    #region Auxiliary Methods
    private async Task<bool> UserExists(string cpf)
    {
        var userExist = await _workerRepository.GetByCpf(cpf);

        if (userExist != null!)
        {
            Notify("Este CPF já está em uso.");
            return true;
        };
        return false;
    }
    private bool SubscriberIdIsValid(Guid? subscriberId)
    {
        if (subscriberId is null || subscriberId == Guid.Empty)
        {
            Notify("Id do assinante, inválido.");
            return false;
        }
        return true;
    }
    private async Task<ValidationResult> PersistData()
    {
        if (!await _workerRepository.UnitOfWork.Commit())
        {
            Notify("Houve um erro ao persistir os dados");
            return _validationResult;
        };
        return _validationResult;
    }
    #endregion
}
