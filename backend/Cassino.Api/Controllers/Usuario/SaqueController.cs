using Cassino.Application.Contracts;
using Cassino.Application.Dtos.V1.Pagamentos;
using Cassino.Application.Dtos.V1.Usuario;
using Cassino.Application.Notification;
using Gerencianet.SDK;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Cassino.Api.Controllers.Usuario;

public class SaqueController : BaseController
{
    private readonly ISaqueService _service;
    public SaqueController(INotificator notificator, ISaqueService service) : base(notificator)
    {
        _service = service;
    }
    
    [HttpPost]
    [SwaggerOperation(Summary = "Realizar saque.", Tags = new [] { "Usuario - Carteira" })]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Cadastrar([FromForm] SaqueDto dto)
    {
        var saque = await _service.Adicionar(dto);
        return CreatedResponse("", saque);
    }

    [HttpPatch]
    [SwaggerOperation(Summary = "Confirmar saque.", Tags = new [] { "Usuario - Carteira" })]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Confirmar()
    {
        
        
        dynamic endpoints = new Endpoints("Client_Id_b25e406f1ee8a087a88b7ad61cfae160b5b915db", "Client_Secret_91ce6d6e91b36a14ddcb754b27bc84f865855418", true);
        
        var body = new
        {
            items = new[] {
                new {
                    name = "Product 1",
                    value = 1000,
                    amount = 2
                }
            },
            shippings = new[] {
                new {
                    name = "Default Shipping Cost",
                    value = 100
                }
            }
        };
        
        
        var response = endpoints.CreateCharge(null, body);
        return Ok();
    }
}