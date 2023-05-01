using Cassino.Application.Contracts;
using Cassino.Application.Dtos.V1.Senha;
using Cassino.Application.Notification;
using Cassino.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Cassino.Api.Controllers.Usuario
{
    [Route("v{version:apiVersion}/Senha/[controller]")]
    public class UsuarioSenhaController : BaseController
    {
        public readonly ISenhaService _senhaService;
        public UsuarioSenhaController(INotificator notificator, ISenhaService senhaService) : base(notificator)
        {
            _senhaService = senhaService;
        }

        [HttpPost("redefinir-senha")]
        public async Task<IActionResult> RedefinirSenha ([FromForm] string email) 
        {
            var usuario = await _senhaService.EmailExiste(email);
            if (usuario != null)
            {
                string link = _senhaService.GerarLinkRedefinicaoSenha(usuario);
                var isEmailEnviado = _senhaService.EmailRedefinicaoSenha(email, link);
                if(isEmailEnviado)
                {
                    return NoContentResponse();
                }
                return BadRequest();
            }
            return BadRequest();
        }

        [HttpPost("alterar-senha-deslogado/codigo={code:string}")]
        public async Task<IActionResult> AlterarSenhaDeslogado(string code, [FromForm] AlterarSenhaDeslogadoDto alterarSenha) { }
    }
}
