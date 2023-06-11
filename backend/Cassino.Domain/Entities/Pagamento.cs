using Cassino.Domain.Contracts;

namespace Cassino.Domain.Entities;

public class Pagamento : Entity, ISoftDelete, IAggregateRoot
{
    public DateTime DataPagamento { get; set; }
    
    public DateTime DataExpiracaoPagamento { get; set; }
    public string Conteudo { get; set; } = String.Empty;
    public Decimal Valor { get; set; }
    public int UsuarioId { get; set; }
    public int PagamentoId { get; set; }
    public bool Aprovado { get; set; }
    public bool Desativado { get; set; }

    public virtual Usuario Usuario { get; set; } = null!;
}