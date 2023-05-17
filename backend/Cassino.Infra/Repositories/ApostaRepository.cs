using Cassino.Domain.Contracts;
using Cassino.Domain.Contracts.Paginacao;
using Cassino.Domain.Contracts.Repositories;
using Cassino.Domain.Entities;
using Cassino.Infra.Abstractions;
using Cassino.Infra.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Cassino.Infra.Repositories
{
    internal class ApostaRepository : Repository<Aposta>, IApostaRepository
    {
        protected readonly BaseApplicationDbContext Context;
        public ApostaRepository(BaseApplicationDbContext context) : base(context)
        {
            Context = context;
        }

        public async Task Adicionar(Aposta aposta)
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
