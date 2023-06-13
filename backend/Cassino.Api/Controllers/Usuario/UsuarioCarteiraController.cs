using Cassino.Application.Contracts;
using Cassino.Application.Dtos.V1.Pagamentos;
using Cassino.Application.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using Swashbuckle.AspNetCore.Annotations;

namespace Cassino.Api.Controllers.Usuario;

[Route("v{version:apiVersion}/Cliente/[controller]")]
public class UsuarioCarteiraController : BaseController
{
    private readonly IUsuarioCarteiraService _service;
    public UsuarioCarteiraController(INotificator notificator, IUsuarioCarteiraService service) : base(notificator)
    {
        _service = service;
    }

    [AllowAnonymous]
    [HttpPost]
    [SwaggerOperation(Summary = "Realizar depósito pix de um Cliente.", Tags = new [] { "Usuario - Carteira" })]
    [ProducesResponseType(typeof(PixDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> PagarComPix([FromBody] DadosPagamentoPixDto dto)
    {
        var pix = await  _service.Deposito(dto);
        return OkResponse(pix);
    }
    
    [AllowAnonymous]
    [HttpGet]
    [SwaggerOperation(Summary = "Ver últimos depósitos pix de um Cliente.", Tags = new [] { "Usuario - Carteira" })]
    public async Task<IActionResult> UltimosPagamentos()
    {
        var client = new RestClient("https://api.pagar.me/core/v5/orders");
        var request = new RestRequest();
        request.Method = Method.Get;
        request.AddHeader("accept", "application/json");
        request.AddHeader("content-type", "application/json");
        request.AddHeader("authorization", "Basic c2tfTlE5V3FNa1RRaWFiSk0ycjo=");
        RestResponse response = await client.ExecuteAsync(request);
        return Ok(response.Content);
    }

    [AllowAnonymous]
    [HttpPost("webhook/")]
    [SwaggerOperation(Summary = "Webhook confirmação pix.", Tags = new[] { "Usuario - Carteira" })]
    public async Task<IActionResult> PixWebHook()
    {
        var retorno = Request.Body.ToString();
        await _service.WebhookPix(retorno);
        return OkResponse();
    }
    
    [HttpPost("/saque")]
    public async Task<IActionResult> Saque()
    {

        return Ok();
    }
}