using EMS.Subscription.API.Model;
using FluentValidation.Results;

namespace EMS.Subscription.API.Business;

public interface IPlanUserService
{
    Task<ValidationResult> AddPlanUser(PlanUser user);
    Task<IEnumerable<PlanUser>> GetAllPlanUsers();
    Task<PlanUser> GetByUserId(Guid userId);
}