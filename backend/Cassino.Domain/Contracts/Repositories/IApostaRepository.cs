using Cassino.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cassino.Domain.Contracts.Repositories
{
    public interface IApostaRepository : IRepository<Aposta>
    {
        void Adicionar(Aposta aposta);
        Task<List<Aposta>> ObterPorUsuario(Usuario usuario);
        Task<List<Aposta>> ObterTodas();
    }
}
