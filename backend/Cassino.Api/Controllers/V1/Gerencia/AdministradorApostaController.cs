using Cassino.Application.Contracts;
using Cassino.Application.Dtos.V1.Auth;
using Cassino.Application.Notification;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Cassino.Api.Controllers.V1.Gerencia
{
    [Route("v{version:apiVersion}/Aposta/[controller]")]
    public class AdministradorApostaController : BaseController
    {
        private readonly IApostaService _apostaService;
        public AdministradorApostaController(IApostaService apostaService, INotificator notificator) : base(notificator)
        {
            _apostaService = apostaService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Lista todas as apostas registradas.", Tags = new[] { "Administrador - Listagem de Apostas" })]
        [ProducesResponseType(typeof(AdministradorAutenticadoDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> BuscarTodasApostas()
        {
            var apostas = await _apostaService.ObterTodasApostas();
            return Ok(apostas);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Lista todas as apostas registradas de um jogador/usuário.", Tags = new[] { "Administrador - Listagem de Apostas" })]
        [ProducesResponseType(typeof(AdministradorAutenticadoDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> BuscarApostaUsuario(int id)
        {
            var apostasUsuario = await _apostaService.ObterApostasDeUsuario(id);
            return Ok(apostasUsuario);
        }

    }
}
