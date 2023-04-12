using Cassino.Domain.Contracts.Paginacao;
using Cassino.Domain.Entities;

namespace Cassino.Domain.Contracts.Repositories;

public interface IUsuarioRepository : IRepository<Usuario>
{
    void Adicionar(Usuario usuario);
    void Alterar(Usuario usuario);
    Task<Usuario?> ObterPorId(int id);
    Task<Usuario?> ObterPorEmail(string email);
    Task<Usuario?> ObterPorCpf(string cpf);
    Task<IResultadoPaginado<Usuario>> Buscar(IBuscaPaginada<Usuario> filtro);
}