using EMS.Core.Data;
using EMS.Subscription.API.Model;
using Microsoft.EntityFrameworkCore;

namespace EMS.Subscription.API.Data.Repository
{
    public class PlanRepository : IPlanRepository
    {
        private readonly SubscriptionContext _context;

        public PlanRepository(SubscriptionContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => (IUnitOfWork)_context;

        public async Task<IEnumerable<Plan>> GetAll()
        {
            return await _context.Plans.AsNoTracking().ToListAsync();
        }

        public async Task<Plan> GetById(Guid id)
        {
            return await _context.Plans.FindAsync(id) ?? new Plan();
        }

        public void AddPlan(Plan produto)
        {
            _context.Plans.Add(produto);
        }

        public void UpdatePlan(Plan produto)
        {
            _context.Plans.Update(produto);
        }

        public async Task EnablePlan(Guid id, bool enable)
        {
            var planDb = await GetById(id);
            if (enable)
            {
                planDb.IsActive = true;
            }
            _context.Plans.Update(planDb);
        }

        public async Task DisablePlan(Guid id, bool disable)
        {
            var planDb = await GetById(id);
            if (disable)
            {
                planDb.IsActive = false;
            }
            _context.Plans.Update(planDb);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}