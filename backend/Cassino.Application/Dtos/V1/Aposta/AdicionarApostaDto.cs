using Cassino.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cassino.Application.Dtos.V1.Aposta
{
    public class AdicionarApostaDto
    {
        public int IdUsuario { get; set; }
        public decimal Valor { get; set; }
        public GameName Jogo { get; set; }
    }
}
