using Cassino.Domain.Entities;

namespace Cassino.Domain.Contracts.Repositories;

public interface ISaqueRepository: IRepository<Saque>
{
    void Adicionar(Saque saque);
    void Alterar(Saque saque);
}