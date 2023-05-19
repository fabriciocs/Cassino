using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cassino.Application.Dtos.V1.Senha
{
    public class AlterarSenhaDto
    {
        public string NovaSenha { get; set; } = null!;
        public string ConfirmarNovaSenha { get; set; } = null!;
    }
}
