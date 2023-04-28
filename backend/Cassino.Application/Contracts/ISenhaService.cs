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
        Task<bool> EmailExiste(string email);
        Task<string> CriarTokenRedefinicaoSenha(Usuario usuario);
        Task<bool> EmailRedefinicaoSenha(Usuario user, string token);
    }
}
