using Cassino.Domain.Entities;

namespace Cassino.Domain.Contracts.Repositories;

public interface IPagamentosRepository : IRepository<Pagamento>
{
    void Adicionar(Pagamento usuario);
}