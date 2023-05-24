using Cassino.Domain.Contracts;
using System.ComponentModel;

namespace Cassino.Domain.Entities
{
    public class Aposta : Entity, IAggregateRoot
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public decimal Valor { get; set; }
        public DateTime Data { get; set; } = DateTime.UtcNow;
        public GameName Jogo { get; set; }
        public Usuario Usuario { get; set; } = null!;
    }

    public enum GameName
    {
        [Description("Game: Mines")]
        Mines = 1,
        [Description("Game: Aviator")]
        Aviator = 2,
        [Description("Game: Roleta")]
        Roleta = 3,
        [Description("Game: Penalty")]
        Penalty = 4,
        [Description("Game: Dados")]
        Dados = 5,
        [Description("Game: Space Man")]
        SpaceMan = 6,
        [Description("Game: Football Studio")]
        FootballStudio = 7,
        [Description("Game: Black Jack")]
        BlackJack = 8
    }
}
