using Cassino.Domain.Contracts.Paginacao;
using Cassino.Domain.Contracts.Repositories;
using Cassino.Domain.Entities;
using Cassino.Infra.Abstractions;
using Cassino.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Cassino.Infra.Repositories;

public class ClienteRepository : Repository<Cliente>, IClienteRepository
{
    public ClienteRepository(BaseApplicationDbContext context) : base(context)
    {
    }

    public void Adicionar(Cliente cliente)
    {
        Context.Clientes.Add(cliente);
    }

    public void Alterar(Cliente cliente)
    {
        Context.Clientes.Update(cliente);
    }

    public async Task<Cliente?> ObterPorId(int id)
    {
        return await Context.Clientes.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Cliente?> ObterPorEmail(string email)
    {
        return await Context.Clientes.FirstOrDefaultAsync(c => c.Email == email);
    }

    public async Task<Cliente?> ObterPorCpf(string cpf)
    {
        return await Context.Clientes.FirstOrDefaultAsync(c => c.Cpf == cpf);
    }

    public async Task<IResultadoPaginado<Cliente>> Buscar(IBuscaPaginada<Cliente> filtro)
    {
        var query = Context.Clientes.AsQueryable();
        return await base.Buscar(query, filtro);
    }
}