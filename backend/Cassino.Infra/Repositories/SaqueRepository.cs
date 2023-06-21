using Cassino.Domain.Contracts.Repositories;
using Cassino.Domain.Entities;
using Cassino.Infra.Abstractions;
using Cassino.Infra.Context;

namespace Cassino.Infra.Repositories;

public class SaqueRepository : Repository<Saque>, ISaqueRepository
{
    public SaqueRepository(BaseApplicationDbContext context) : base(context)
    {
    }

    public void Adicionar(Saque saque)
    {
        Context.Saques.Add(saque);
    }

    public void Alterar(Saque saque)
    {
        Context.Saques.Update(saque);
    }
}