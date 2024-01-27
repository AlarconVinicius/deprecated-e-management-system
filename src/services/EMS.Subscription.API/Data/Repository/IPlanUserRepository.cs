using EMS.Core.Data;
using EMS.Subscription.API.Model;

namespace EMS.Subscription.API.Data.Repository;

public interface IPlanUserRepository : IRepository<PlanUser>
{
    Task<PlanUser> GetById(Guid id);
    Task<PlanUser> GetByUserCpf(string cpf);
    Task<IEnumerable<PlanUser>> GetAll();
    void AddPlanUser(PlanUser planUser);
    void UpdatePlanUser(PlanUser planUser);
    Task DisablePlanUser(Guid id, bool disable);
    Task EnablePlanUser(Guid id, bool enable);
}