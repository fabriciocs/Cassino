using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cassino.Domain.Entities
{
    public class Aposta
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public decimal Valor { get; set; }
        public DateOnly Data { get; set; }
        public GameName Jogo { get; set; }
    }

    public enum GameName
    {
        Mines,
        Aviator,
        Roleta,
        Penalty,
        Dados,
        SpaceMan,
        FootballStudio,
        BlackJack
    }
}
