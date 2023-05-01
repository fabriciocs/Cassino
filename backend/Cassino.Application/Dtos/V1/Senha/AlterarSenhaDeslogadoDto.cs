using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cassino.Application.Dtos.V1.Senha
{
    public class AlterarSenhaDeslogadoDto
    {
        public string NovaSenha { get; set; }
        public string ConfirmarNovaSenha { get; set; }
    }
}
