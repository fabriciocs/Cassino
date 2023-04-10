using Cassino.Domain.Contracts.Paginacao;
using Cassino.Domain.Entities;

namespace Cassino.Domain.Contracts.Repositories;

public interface IAdministradorRepository : IRepository<Administrador>
{
    void Adicionar(Administrador administrador);
    void Alterar(Administrador administrador);
    Task<Administrador?> ObterPorId(int id);
    Task<Administrador?> ObterPorEmail(string email);
    Task<IResultadoPaginado<Administrador>> Buscar(IBuscaPaginada<Administrador> filtro);
}