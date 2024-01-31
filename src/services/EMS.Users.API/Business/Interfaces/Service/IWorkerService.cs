using EMS.Users.API.Models;
using FluentValidation.Results;

namespace EMS.Users.API.Business;

public interface IWorkerService
{
    Task<ValidationResult> AddWorker(Worker worker);
    Task<ValidationResult> UpdateWorker(Worker worker);
    Task<ValidationResult> DeleteWorker(Guid id);
    Task<IEnumerable<Worker>> GetAllWorkers();
    Task<Worker> GetByCpf(string cpf);
}