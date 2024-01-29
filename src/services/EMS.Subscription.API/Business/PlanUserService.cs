using EMS.Subscription.API.Data.Repository;
using EMS.Subscription.API.Model;
using EMS.Subscription.API.Models.Validations;
using EMS.WebAPI.Core.Services;
using FluentValidation.Results;

namespace EMS.Subscription.API.Business;

public class PlanUserService : MainService, IPlanUserService
{
    private readonly IPlanUserRepository _planUserRepository;
    private readonly IPlanRepository _planRepository;
    public PlanUserService(IPlanUserRepository planUserRepository, IPlanRepository planRepository, INotifier notifier) : base(notifier)
    {
        _planUserRepository = planUserRepository;
        _planRepository = planRepository;
    }

    public async Task<ValidationResult> AddPlanUser(PlanUser planUser)
    {
        if (!ExecuteValidation(new PlanUserValidation(), planUser)) return _validationResult;

        var planExist = await _planRepository.GetById(planUser.PlanId);
        var planUserExist = await _planUserRepository.GetByUserCpf(planUser.UserCpf);

        if (planExist is null)
        {
            Notify("Plano não encontrado.");
            return _validationResult;
        }
        if (planUserExist != null!)
        {
            Notify($"Cpf '{planUserExist.UserCpf}' vinculado ao plano {planExist!.Title}.");
            return _validationResult;
        }
        _planUserRepository.AddPlanUser(planUser);

        if (!await _planUserRepository.UnitOfWork.Commit())
        {
            Notify("Houve um erro ao persistir os dados");
            return _validationResult;
        };
        return _validationResult;
    }

    public Task<IEnumerable<PlanUser>> GetAllPlanUsers()
    {
        throw new NotImplementedException();
    }

    public Task<PlanUser> GetByUserId(Guid userId)
    {
        throw new NotImplementedException();
    }
}
