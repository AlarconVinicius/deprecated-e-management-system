using EMS.Core.Data;
using EMS.Users.API.Business.Interfaces.Repository;
using EMS.Users.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EMS.Users.API.Data.Repository;

public class WorkerRepository : IWorkerRepository
{
    private readonly UsersContext _context;

    public WorkerRepository(UsersContext context)
    {
        _context = context;
    }

    public IUnitOfWork UnitOfWork => _context;

    public async Task<IEnumerable<Worker>> GetAllWorkers()
    {
        return await _context.Workers.AsNoTracking().ToListAsync();
    }

    public async Task<Worker> GetById(Guid id)
    {
        return await _context.Workers.FirstOrDefaultAsync(c => c.Id == id) ?? null!;
    }

    public async Task<Worker> GetByCpf(string cpf)
    {
        return await _context.Workers.FirstOrDefaultAsync(c => c.Cpf.Number == cpf) ?? null!;
    }

    public void AddWorker(Worker worker)
    {
        _context.Workers.Add(worker);
    }

    public void UpdateWorker(Worker worker)
    {
        _context.Workers.Update(worker);
    }

    public void DeleteWorker(Worker worker)
    {
        _context.Workers.Remove(worker);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}