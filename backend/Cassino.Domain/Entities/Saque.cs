using Cassino.Domain.Contracts;

namespace Cassino.Domain.Entities;

public class Saque :  Entity, ISoftDelete, IAggregateRoot
{
    public Decimal Valor { get; set; }
    public bool Aprovado { get; set; }
    public DateTime DataSaque { get; set; }
    public int UsuarioId { get; set; }
    public bool Desativado { get; set; }

    public virtual Usuario Usuario { get; set; } = null!;
}