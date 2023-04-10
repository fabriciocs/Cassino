using Cassino.Domain.Contracts;
using Cassino.Domain.Validation;
using FluentValidation.Results;

namespace Cassino.Domain.Entities;

public class Administrador : Entity, IAggregateRoot, ISoftDelete
{
    public string Nome { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Senha { get; set; } = null!;
    public bool Desativado { get; set; }

    public override bool Validar(out ValidationResult validationResult)
    {
        validationResult = new AdministradorValidator().Validate(this);
        return validationResult.IsValid;
    }
}