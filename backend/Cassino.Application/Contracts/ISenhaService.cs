using Cassino.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cassino.Application.Contracts
{
    public interface ISenhaService
    {
        Task<Usuario> EmailExiste(string email);
        string GerarLinkRedefinicaoSenha(Usuario usuario);
        bool EmailRedefinicaoSenha(string email, string link);
        Task<Usuario?> CodigoExiste(string codigo);
    }
}
