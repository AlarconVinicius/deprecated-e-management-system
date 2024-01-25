using EMS.Core.DomainObjects;
using FluentValidation;

namespace EMS.Users.API.Models.Validations;

public class UserValidation : AbstractValidator<User>
{
    public UserValidation()
    {
        RuleFor(c => c.Id)
            .NotEqual(Guid.Empty)
            .WithMessage("Id do usuário inválido");

        RuleFor(c => c.Name)
            .NotEmpty()
            .WithMessage("O nome do usuário não foi informado");

        RuleFor(c => c.Cpf.Number)
            .Must(TerCpfValido)
            .WithMessage("O CPF informado não é válido.");

        RuleFor(c => c.Email.Address)
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
