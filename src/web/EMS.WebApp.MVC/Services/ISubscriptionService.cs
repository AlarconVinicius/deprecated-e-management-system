using EMS.WebApp.MVC.Models;

namespace EMS.WebApp.MVC.Services;

public interface ISubscriptionService
{
    Task<IEnumerable<PlanViewModel>> GetAll();
    Task<PlanViewModel> GetById(Guid id);
}