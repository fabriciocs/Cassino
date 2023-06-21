namespace Cassino.Application.Dtos.V1.Pagamentos;

public class SaqueDto
{
    public Decimal Valor { get; set; }
    public bool Aprovado { get; set; }
    public int UsuarioId { get; set; }
}