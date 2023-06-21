using Cassino.Application.Dtos.V1.Pagamentos;
using Cassino.Domain.Entities;

namespace Cassino.Application.Contracts;

public interface ISaqueService
{
    Task<Saque?> Adicionar(SaqueDto saque);
    Task<Saque?> Alterar(Saque saque);
    Task Confirmar(int id);
}
