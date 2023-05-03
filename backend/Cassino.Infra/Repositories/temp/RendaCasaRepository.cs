using Cassino.Domain.Contracts;
using Cassino.Domain.Contracts.Repositories.temp;
using Cassino.Domain.Entities.temp;
using Cassino.Infra.Context;

namespace Cassino.Infra.Repositories.temp;

public class RendaCasaRepository : IRendaCasaRepository
{
    protected readonly BaseApplicationDbContext Context;
    public RendaCasaRepository(BaseApplicationDbContext context)
    {
        Context = context;
    }

    public IUnitOfWork UnitOfWork { get; }

    public void Adicionar(Renda renda)
    {
        Context.Rendas.Add(renda);
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}