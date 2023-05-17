using Cassino.Application.Contracts;
using Cassino.Application.Notification;
using Microsoft.AspNetCore.Mvc;

namespace Cassino.Api.Controllers.Usuario
{
    [Route("v{version:apiVersion}/Aposta/[controller]")]
    public class UsuarioApostaController : BaseController
    {
        private readonly IApostaService _apostaService;
        public UsuarioApostaController(IApostaService apostaService, INotificator notificator) : base(notificator)
        {
            _apostaService = apostaService;
        }

    }
}
