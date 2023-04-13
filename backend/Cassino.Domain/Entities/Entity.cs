using Cassino.Domain.Contracts;
using FluentValidation.Results;

namespace Cassino.Domain.Entities;

public abstract class Entity : BaseEntity, ITracking
{
    public int? CriadoPor { get; set; }
    public bool CriadoPorAdmin { get; set; }
    public int? AtualizadoPor { get; set; }
    public bool AtualizadoPorAdmin { get; set; }

    public virtual bool Validar(out ValidationResult validationResult)
    {
        validationResult = new ValidationResult();
        return validationResult.IsValid;
    }
}

public abstract class EntityNotTracked : BaseEntity
{
    
}

public abstract class BaseEntity : IEntity
{
    public int Id { get; set; }
}