using Cassino.Application.Contracts;
using Cassino.Application.Dtos.V1.Senha;
using Cassino.Application.Notification;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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

        [HttpPost("solicitar-redefincao-de-senha")]
        [SwaggerOperation(Summary = "Envia um e-mail de redefinição de senha para o usuario deslogado.", Tags = new[] { "Usuario - Cliente - Senha" })]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SolicitarRedefinicaoSenha([FromForm] string email)
        {
            var resultado = await _senhaService.Solicitar(email);
            return resultado ? NoContentResponse() : BadRequest();
        }

        [HttpPost("redefinir-senha/codigo={code}")]
        [SwaggerOperation(Summary = "Valida e salva uma nova senha para o usuário deslogado.", Tags = new[] { "Usuario - Cliente - Senha" })]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RedefinirSenha(string code, [FromForm] AlterarSenhaDto novaSenha) 
        {
            var resultado = await _senhaService.Redefinir(code, novaSenha);
            return resultado ? NoContentResponse() : BadRequest();
        }

        [HttpPut("alterar-senha")]
        [SwaggerOperation(Summary = "Verifica senha antiga e atualiza a senha do usuário logado.", Tags = new[] { "Usuario - Cliente - Senha" })]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AlterarSenha([FromForm] string senhaAntiga, [FromForm] AlterarSenhaDto alterarSenhaDto)
        {
            var resultado = await _senhaService.AlterarSenhaLogin(senhaAntiga, alterarSenhaDto);
            return resultado ? NoContentResponse() : BadRequest();
        }
    }
}
