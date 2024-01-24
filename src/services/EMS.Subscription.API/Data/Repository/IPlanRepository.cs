using EMS.Core.Data;
using EMS.Subscription.API.Model;

namespace EMS.Subscription.API.Data.Repository
{
    public interface IPlanRepository : IRepository<Plan>
    {
        Task<Plan> GetById(Guid id);
        Task<IEnumerable<Plan>> GetAll();
        void AddPlan(Plan produto);
        void UpdatePlan(Plan produto);
        Task DisablePlan(Guid id, bool disable);
        Task EnablePlan(Guid id, bool enable);
    }
}