using EMS.Core.DomainObjects;
using EMS.Subscription.API.Model;
using FluentValidation;

namespace EMS.Subscription.API.Models.Validations;

public class PlanUserValidation : AbstractValidator<PlanUser>
{
    public PlanUserValidation()
    {
        RuleFor(c => c.PlanId)
            .NotEqual(Guid.Empty)
            .WithMessage("Id do plano inválido");

        RuleFor(c => c.ClientId)
           .NotEqual(Guid.Empty)
           .WithMessage("Id do cliente inválido");

        RuleFor(c => c.UserName)
            .NotEmpty()
            .WithMessage("O nome do usuário não foi informado");

        RuleFor(c => c.UserCpf)
            .Must(TerCpfValido)
            .WithMessage("O CPF informado não é válido.");

        RuleFor(c => c.UserEmail)
            .Must(TerEmailValido)
            .WithMessage("O e-mail informado não é válido.");
    }

    protected static bool TerCpfValido(string cpf)
    {
        return Cpf.Validate(cpf);
    }

    protected static bool TerEmailValido(string email)
    {
        return Email.Validate(email);
    }
}
