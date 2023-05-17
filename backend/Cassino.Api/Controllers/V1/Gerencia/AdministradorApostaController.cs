using Cassino.Application.Contracts;
using Cassino.Application.Notification;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> BuscarTodasApostas()
        {
            var apostas = _apostaService.ObterTodasApostas();
            return Ok(apostas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarApostaUsuario(int id)
        {
            var apostasUsuario = _apostaService.ObterApostasDeUsuario(id);
            return Ok(apostasUsuario);
        }

    }
}
