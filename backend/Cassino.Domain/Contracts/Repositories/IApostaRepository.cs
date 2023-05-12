using Cassino.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cassino.Domain.Contracts.Repositories
{
    public interface IApostaRepository
    {
        void NovoRegistro(Aposta NovaAposta);
    }
}
