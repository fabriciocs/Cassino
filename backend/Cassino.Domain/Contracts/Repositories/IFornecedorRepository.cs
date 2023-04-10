using Cassino.Domain.Contracts.Paginacao;
using Cassino.Domain.Entities;

namespace Cassino.Domain.Contracts.Repositories;

public interface IFornecedorRepository : IRepository<Fornecedor>
{
    void Adicionar(Fornecedor fornecedor);
    void Alterar(Fornecedor fornecedor);
    Task<Fornecedor?> ObterPorId(int id);
    Task<Fornecedor?> ObterPorEmail(string email);
    Task<Fornecedor?> ObterPorCpf(string cpf);
    Task<Fornecedor?> ObterPorCnpj(string cnpj);
    Task<IResultadoPaginado<Fornecedor>> Buscar(IBuscaPaginada<Fornecedor> filtro);
}