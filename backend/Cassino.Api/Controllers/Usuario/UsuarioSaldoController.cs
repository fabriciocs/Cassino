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

        [HttpGet]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Consulta Saldo de um Cliente.", Tags = new[] { "Saldo - Usuario - Cliente" })]
        [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> BuscarSaldo([FromForm] int id)
        {
            var saldo = await _usuarioService.BuscarSaldo(id);
            return saldo == null ? NotFound() : Ok(saldo.Saldo);
        }
    }
}
