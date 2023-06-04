using Cassino.Domain.Entities;

namespace Cassino.Application.Dtos.V1.Aposta
{
    public class VMApostaDto
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public decimal Valor { get; set; }
        public DateTime Data { get; set; } = DateTime.Now;
        public GameName Jogo { get; set; }
        //public bool EhApostaInicial {get; set}
    }
}
