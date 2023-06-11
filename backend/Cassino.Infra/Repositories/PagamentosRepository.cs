using Cassino.Domain.Contracts.Repositories;
using Cassino.Domain.Entities;
using Cassino.Infra.Abstractions;
using Cassino.Infra.Context;

namespace Cassino.Infra.Repositories;

public class PagamentosRepository : Repository<Pagamento>, IPagamentosRepository
{
    public PagamentosRepository(BaseApplicationDbContext context) : base(context)
    {
    }

    public void Adicionar(Pagamento pagamento)
    {
        Context.Pagamentos.Add(pagamento);
    }
}