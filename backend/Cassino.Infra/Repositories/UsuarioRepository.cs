using Cassino.Domain.Contracts.Paginacao;
using Cassino.Domain.Contracts.Repositories;
using Cassino.Domain.Entities;
using Cassino.Infra.Abstractions;
using Cassino.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Cassino.Infra.Repositories;

public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(BaseApplicationDbContext context) : base(context)
    {
    }

    public void Adicionar(Usuario usuario)
    {
        Context.Clientes.Add(usuario);
    }

    public void Alterar(Usuario usuario)
    {
        Context.Clientes.Update(usuario);
    }

    public async Task<Usuario?> ObterPorId(int id)
    {
        return await Context.Clientes.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Usuario?> ObterPorEmail(string email)
    {
        return await Context.Clientes.FirstOrDefaultAsync(c => c.Email == email);
    }

    public async Task<Usuario?> ObterPorCpf(string cpf)
    {
        return await Context.Clientes.FirstOrDefaultAsync(c => c.Cpf == cpf);
    }

    public async Task<IResultadoPaginado<Usuario>> Buscar(IBuscaPaginada<Usuario> filtro)
    {
        var query = Context.Clientes.AsQueryable();
        return await base.Buscar(query, filtro);
    }
}