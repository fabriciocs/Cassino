using Cassino.Application.Contracts;
using Cassino.Application.Dtos.V1.Saldo;
using Cassino.Application.Dtos.V1.Usuario;
using Cassino.Application.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Cassino.Api.Controllers.Usuario
{
    [Route("v{version:apiVersion}/Saldo/[controller]")]
    [ApiController]
    public class UsuarioSaldoController : BaseController
    {
        private readonly IUsuarioService _usuarioService;
        public UsuarioSaldoController(INotificator notificator, IUsuarioService usuarioService) : base(notificator)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet("buscar-saldo/{id}")]
        [SwaggerOperation(Summary = "Consulta Saldo de um Cliente.", Tags = new[] { "Usuario - Cliente - Saldo" })]
        [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> BuscarSaldo([FromQuery] int id)
        {
            var saldo = await _usuarioService.BuscarSaldo(id);
            return saldo == null ? NotFound() : Ok(saldo.Saldo);
        }

        [HttpPut]
        [SwaggerOperation(Summary = "Atualiza o Saldo de um Cliente.", Tags = new[] { "Usuario - Cliente - Saldo" })]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AtualizarSaldo([FromForm] SaldoUsuarioDto saldoUsuarioDto)
        {
            var usuarioAtualizado = await _usuarioService.AtualizarSaldo(saldoUsuarioDto);
            return usuarioAtualizado == true ? Ok() : BadRequest();
        }
    }
}
