namespace Cassino.Domain.Contracts;

public interface IUnitOfWork
{
    Task<bool> Commit();
}