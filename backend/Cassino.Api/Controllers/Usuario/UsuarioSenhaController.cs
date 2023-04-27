using Cassino.Application.Contracts;
using Cassino.Application.Notification;
using Microsoft.AspNetCore.Mvc;

namespace Cassino.Api.Controllers.Usuario
{
    [Route("v{version:apiVersion}/Senha/[controller]")]
    public class UsuarioSenhaController : BaseController
    {
        private readonly IUsuarioService _clienteRepository;
        public UsuarioSenhaController(INotificator notificator, IUsuarioService clienteRepository) : base(notificator)
        {
            _clienteRepository = clienteRepository;
        }

        [HttpPost]
        public async Task<IActionResult> RecuperarSenha([FromForm] string email)
        {

        }
    }
}
