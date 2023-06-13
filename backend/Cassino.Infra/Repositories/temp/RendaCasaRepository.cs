using Cassino.Domain.Contracts;
using Cassino.Domain.Contracts.Repositories.temp;
using Cassino.Domain.Entities.temp;
using Cassino.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Cassino.Infra.Repositories.temp;

public class RendaCasaRepository : IRendaCasaRepository
{
    private bool _isDisposed;
    protected readonly BaseApplicationDbContext Context;
    public RendaCasaRepository(BaseApplicationDbContext context)
    {
        Context = context;
    }
    
    public IUnitOfWork UnitOfWork => Context;
    public void Adicionar(Renda renda)
    {
        Context.Rendas.Add(renda);
    }

    public void AtualizarSaldoCasa(Renda renda)
    {
        Context.Rendas.Update(renda);
    }

    public Renda ObterCasa()
    {
        return Context.Rendas.FirstOrDefault();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_isDisposed) return;

        if (disposing)
        {
            Context.Dispose();
        }

        _isDisposed = true;
    }
    
    ~RendaCasaRepository()
    {
        Dispose(false);
    }
}