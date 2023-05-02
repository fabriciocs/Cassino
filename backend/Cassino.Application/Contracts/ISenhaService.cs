using Cassino.Application.Dtos.V1.Senha;
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
        Task<string?> GerarLinkRedefinicaoSenha(Usuario usuario);
        bool EmailRedefinicaoSenha(string email, string link);
        Task<Usuario?> CodigoExiste(string codigo);
        bool VerificarSenha(AlterarSenhaDto novaSenha);
        Task<bool> SalvarNovaSenha(Usuario usuario, AlterarSenhaDto alterarSenha);
        Task<bool> AlterarSenhaLogin(string novaSenha, AlterarSenhaDto alterarSenhaDto);
    }
}
