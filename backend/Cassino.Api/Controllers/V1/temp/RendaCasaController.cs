
using Cassino.Application.Contracts.temp;
using Cassino.Application.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Cassino.Api.Controllers.V1.temp;

public class RendaCasaController : BaseController
{
    private readonly IRendaCasaService _rendaCasaService;
    public RendaCasaController(INotificator notificator, IRendaCasaService rendaCasaService) : base(notificator)
    {
        _rendaCasaService = rendaCasaService;
    }
    
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Adicionar([FromBody] decimal valor)
    {
        await _rendaCasaService.Adicionar(valor);
        return OkResponse();
    }

    [AllowAnonymous]
    [HttpPost("alterar")]
    [SwaggerOperation(Summary = "Define um valor total para a Casa.", Tags = new[] { "RendaCasa - TESTES" })]
    public async Task<IActionResult> MudarRendaCasa([FromBody] decimal valor)
    {
        var valorTotal = await _rendaCasaService.MudarRendaCasa(valor);
        if (valorTotal == null)
            return BadRequest("Houver um problema ao atualizar o saldo da casa.");
        return Ok(valorTotal);
    }

    [AllowAnonymous]
    [HttpGet("obter")]
    [SwaggerOperation(Summary = "Retorna o valor total da Casa.", Tags = new[] { "RendaCasa - TESTES" })]
    public async Task<IActionResult> ObterRendaCasa()
    {
        var RendaCasa = await _rendaCasaService.ObterRendaCasa();
        return Ok(RendaCasa);
    }
}