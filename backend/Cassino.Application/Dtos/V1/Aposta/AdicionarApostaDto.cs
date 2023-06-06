using Cassino.Domain.Entities;

namespace Cassino.Application.Dtos.V1.Aposta
{
    public class AdicionarApostaDto
    {
        public int IdUsuario { get; set; }
        public decimal Valor { get; set; }
        public GameName Jogo { get; set; }
        public bool EhApostaInicial { get; set; }
    }
}
