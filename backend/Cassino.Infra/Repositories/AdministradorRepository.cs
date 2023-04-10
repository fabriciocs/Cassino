using Cassino.Domain.Contracts.Paginacao;
using Cassino.Domain.Contracts.Repositories;
using Cassino.Domain.Entities;
using Cassino.Infra.Abstractions;
using Cassino.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Cassino.Infra.Repositories;

public class AdministradorRepository : Repository<Administrador>, IAdministradorRepository
{
    public AdministradorRepository(BaseApplicationDbContext context) : base(context)
    {
    }

    public void Adicionar(Administrador administrador)
    {
        Context.Administradores.Add(administrador);
    }

    public void Alterar(Administrador administrador)
    {
        Context.Administradores.Update(administrador);
    }

    public async Task<Administrador?> ObterPorId(int id)
    {
        return await Context.Administradores.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Administrador?> ObterPorEmail(string email)
    {
        return await Context.Administradores.FirstOrDefaultAsync(c => c.Email == email);
    }

    public async Task<IResultadoPaginado<Administrador>> Buscar(IBuscaPaginada<Administrador> filtro)
    {
        var query = Context.Administradores.AsQueryable();
        return await base.Buscar(query, filtro);
    }
}