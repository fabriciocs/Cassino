
using Cassino.Application.Contracts.temp;
using Cassino.Application.Notification;
using Cassino.Application.Services.temp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    [HttpGet]
    public IActionResult ObterRendaCasa()
    {
        var RendaCasa = (_rendaCasaService.ObterRendaCasa());
        return Ok(RendaCasa);
    }
}