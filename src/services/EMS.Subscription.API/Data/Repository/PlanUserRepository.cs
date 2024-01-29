using EMS.Core.Data;
using EMS.Subscription.API.Model;
using Microsoft.EntityFrameworkCore;

namespace EMS.Subscription.API.Data.Repository;

public class PlanUserRepository : IPlanUserRepository
{
    private readonly SubscriptionContext _context;

    public PlanUserRepository(SubscriptionContext context)
    {
        _context = context;
    }

    public IUnitOfWork UnitOfWork => (IUnitOfWork)_context;

    public async Task<IEnumerable<PlanUser>> GetAll()
    {
        return await _context.PlanUsers.AsNoTracking().ToListAsync();
    }

    public async Task<PlanUser> GetById(Guid id)
    {
        return await _context.PlanUsers.FindAsync(id) ?? null!;
    }
    public async Task<PlanUser> GetByUserCpf(string cpf)
    {
        return await _context.PlanUsers.Include(pu => pu.Plan).FirstOrDefaultAsync(pu => pu.UserCpf == cpf) ?? null!;
    }

    public void AddPlanUser(PlanUser produto)
    {
        _context.PlanUsers.Add(produto);
    }

    public void UpdatePlanUser(PlanUser produto)
    {
        _context.PlanUsers.Update(produto);
    }

    public async Task EnablePlanUser(Guid id, bool enable)
    {
        var planDb = await GetById(id);
        if (enable)
        {
            planDb.IsActive = true;
        }
        _context.PlanUsers.Update(planDb);
    }

    public async Task DisablePlanUser(Guid id, bool disable)
    {
        var planDb = await GetById(id);
        if (disable)
        {
            planDb.IsActive = false;
        }
        _context.PlanUsers.Update(planDb);
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}