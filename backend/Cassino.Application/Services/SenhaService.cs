using Cassino.Application.Contracts;
using Cassino.Domain.Contracts.Repositories;
using Cassino.Domain.Entities;
using Cassino.Infra.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cassino.Application.Services
{
    public class SenhaService : ISenhaService
    {
        public readonly IUsuarioRepository _usuarioRepository;
        public SenhaService(IUsuarioRepository usuarioRepository) {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<bool> EmailExiste(string email)
        {
            var usuarioExiste = await _usuarioRepository.ObterPorEmail(email);
            if (usuarioExiste != null)
            {
                //Chamao  metodo que gera o codigo e envia por email.
                var codigoEnviado = await EmailRecuperacaoSenha(usuarioExiste);
                if(codigoEnviado)
                    return true;
                return false;
            }
            return false;
        }

        public Task<bool> EmailRecuperacaoSenha(Usuario user)
        {
            throw new NotImplementedException();
        }
    }
}
