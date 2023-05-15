using Cassino.Application.Dtos.V1.Aposta;
using Cassino.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cassino.Application.Contracts
{
    public interface IApostaService
    {
        Task RegistrarAposta(AdicionarApostaDto apostaDto);
        Task<List<Aposta>> ObterApostasDeUsuario(int id);
        Task<List<Aposta>> ObterTodasApostas();
    }
}
