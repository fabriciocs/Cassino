namespace Cassino.Domain.Contracts;

public interface ITracking
{
    public int? CriadoPor { get; set; }
    public bool CriadoPorAdmin { get; set; }
    public int? AtualizadoPor { get; set; }
    public bool AtualizadoPorAdmin { get; set; }
}