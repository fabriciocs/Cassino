using Cassino.Application.Contracts;
using Cassino.Application.Dtos.V1.Usuario;
using Cassino.Application.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Cassino.Api.Controllers.Usuario;

[Route("v{version:apiVersion}/Cliente/[controller]")]
public class ClientesCadastroController : BaseController
{
    private readonly IUsuarioService _usuarioService;
    
    public ClientesCadastroController(INotificator notificator, IUsuarioService usuarioService) : base(notificator)
    {
        _usuarioService = usuarioService;
    }
    
    [HttpPost]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Cadastro de um Cliente.", Tags = new [] { "Usuario - Cliente" })]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Cadastrar([FromForm] CadastrarUsuarioDto dto)
    {
        var usuario = await _usuarioService.Cadastrar(dto);
        return CreatedResponse("", usuario);
    }
    
    [HttpPut("/{id}")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Alterar um Cliente.", Tags = new [] { "Usuario - Cliente" })]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Alterar([FromForm] AlterarUsuarioDto dto, int id)
    {
        var usuario = await _usuarioService.Alterar(id, dto);
        return CreatedResponse("", usuario);
    }
    
    [HttpGet("/{id}")]
    [SwaggerOperation(Summary = "Obter um Cliente.", Tags = new [] { "Usuario - Cliente" })]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var usuario = await _usuarioService.ObterPorId(id);
        return CreatedResponse("", usuario);
    }
}