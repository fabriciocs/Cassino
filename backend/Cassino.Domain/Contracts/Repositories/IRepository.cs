using System.Linq.Expressions;
using Cassino.Domain.Contracts.Paginacao;
using Cassino.Domain.Entities;

namespace Cassino.Domain.Contracts.Repositories;

public interface IRepository<T> : IDisposable where T : BaseEntity, IAggregateRoot
{
    IUnitOfWork UnitOfWork { get; }
    Task<IResultadoPaginado<T>> Buscar(IBuscaPaginada<T> filtro, CancellationToken cancellationToken = default);

    Task<IResultadoPaginado<T>> Buscar(IQueryable<T> queryable, IBuscaPaginada<T> filtro,
        CancellationToken cancellationToken = default);

    Task<List<T>> Buscar(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<T?> FistOrDefault(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<int> Count(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<bool> Any(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
}