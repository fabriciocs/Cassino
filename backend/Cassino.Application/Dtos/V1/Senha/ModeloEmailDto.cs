using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cassino.Application.Dtos.V1.Senha
{
    public class ModeloEmailDto
    {
        public string Nome { get; set; }
        public string Codigo { get; set; }
        public string Url { get; set; }
        public int ExpiracaoEmHoras { get; set; }
    }
}
