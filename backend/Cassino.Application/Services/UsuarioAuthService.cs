using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Cassino.Application.Contracts;
using Cassino.Application.Dtos.V1.Auth;
using Cassino.Application.Notification;
using Cassino.Core.Enums;
using Cassino.Core.Extensions;
using Cassino.Core.Settings;
using Cassino.Domain.Contracts.Repositories;
using Cassino.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Cassino.Application.Services;

public class UsuarioAuthService : BaseService, IUsuarioAuthService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPasswordHasher<Usuario> _passwordHasher;
    private readonly AppSettings _appSettings;

    public UsuarioAuthService(IMapper mapper, INotificator notificator, IUsuarioRepository usuarioRepository,
        IPasswordHasher<Usuario> passwordHasher, IOptions<AppSettings> appSettings) : base(
        mapper, notificator)
    {
        _usuarioRepository = usuarioRepository;
        _passwordHasher = passwordHasher;
        _appSettings = appSettings.Value;
    }

    public async Task<UsuarioAutenticadoDto?> Login(LoginDto loginDto)
    {
        var cliente = await _usuarioRepository.ObterPorEmail(loginDto.Email);
        if (cliente == null)
        {
            Notificator.HandleNotFoundResource();
            return null;
        }

        var result = _passwordHasher.VerifyHashedPassword(cliente, cliente.Senha, loginDto.Senha);
        if (result != PasswordVerificationResult.Failed)
        {
            return new UsuarioAutenticadoDto
            {
                Id = cliente.Id,
                Email = cliente.Email,
                Nome = cliente.Nome,
                Token = await CreateTokenCliente(cliente)
            };
        }

        Notificator.Handle("Combinação de email e senha incorreta!");
        return null;
    }
    
    public Task<string> CreateTokenCliente(Usuario usuario)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(Settings.Settings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nome),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim("TipoUsuario", ETipoUsuario.Comum.ToDescriptionString())
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return Task.FromResult(tokenHandler.WriteToken(token));
    }
}