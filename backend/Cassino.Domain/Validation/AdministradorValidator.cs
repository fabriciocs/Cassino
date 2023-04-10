using Cassino.Domain.Entities;
using FluentValidation;

namespace Cassino.Domain.Validation;

public class AdministradorValidator : AbstractValidator<Administrador>
{
    public AdministradorValidator()
    {
        RuleFor(a => a.Nome)
            .NotEmpty();

        RuleFor(a => a.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(a => a.Senha)
            .NotEmpty();
    }
}