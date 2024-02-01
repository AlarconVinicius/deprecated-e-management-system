using EMS.Core.Data;
using EMS.Users.API.Models;

namespace EMS.Users.API.Business.Interfaces.Repository;

public interface IWorkerRepository : IRepository<Worker>
{
    void AddWorker(Worker worker);
    void UpdateWorker(Worker worker);
    void DeleteWorker(Worker worker);
    Task<IEnumerable<Worker>> GetAllWorkers();
    Task<Worker> GetById(Guid id);
    Task<Worker> GetByCpf(string cpf);
}
