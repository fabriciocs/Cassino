using Cassino.Domain.Contracts.Repositories;
using Cassino.Domain.Entities;
using Cassino.Infra.Abstractions;
using Cassino.Infra.Context;
using Microsoft.EntityFrameworkCore;


namespace Cassino.Infra.Repositories
{
    internal class ApostaRepository : Repository<Aposta>, IApostaRepository
    {
        protected readonly BaseApplicationDbContext Context;
        public ApostaRepository(BaseApplicationDbContext context) : base(context)
        {
            Context = context;
        }

        public async void Adicionar(Aposta aposta)
        {
            await Context.Apostas.AddAsync(aposta);
        }

        public async Task<List<Aposta>> ObterPorUsuario(Usuario usuario)
        {
            return await Context.Apostas
                .Where(a => a.IdUsuario == usuario.Id)
                .OrderByDescending(a => a.Data)
                .ToListAsync();
        }

        public async Task<List<Aposta>> ObterTodas()
        {
            return await Context.Apostas
                .OrderByDescending(a => a.Data)
                .ToListAsync();
        }
    }
}
