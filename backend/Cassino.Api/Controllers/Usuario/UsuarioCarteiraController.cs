using Cassino.Application.Contracts;
using Cassino.Application.Dtos.V1.Pagamentos;
using Cassino.Application.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

namespace Cassino.Api.Controllers.Usuario;

public class UsuarioCarteiraController : BaseController
{
    private readonly IUsuarioCarteiraService _service;
    public UsuarioCarteiraController(INotificator notificator, IUsuarioCarteiraService service) : base(notificator)
    {
        _service = service;
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> PagarComPix([FromBody] DadosPagamentoPixDto dto)
    {
        var pix = await  _service.Deposito(dto);
        // Retorne a resposta adequada para o cliente
        return Ok(pix);
    }
    [AllowAnonymous]
    [HttpGet]
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

    [HttpPost]
    public async Task<IActionResult> Saque()
    {

        return Ok();
    }
}