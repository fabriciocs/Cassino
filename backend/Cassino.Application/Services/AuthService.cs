using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Cassino.Application.Contracts;
using Cassino.Application.Dtos.V1.Auth;
using Cassino.Application.Notification;
using Cassino.Core.Enums;
using Cassino.Core.Extensions;
using Cassino.Domain.Contracts.Repositories;
using Cassino.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Cassino.Application.Services;

public class AuthService : BaseService, IAuthService
{
    private readonly IAdministradorRepository _administradorRepository;
    private readonly IPasswordHasher<Administrador> _admPasswordHasher;
    
    public AuthService(IMapper mapper, INotificator notificator, IAdministradorRepository administradorRepository, IPasswordHasher<Administrador> admPasswordHasher) : base(mapper, notificator)
    {
        _administradorRepository = administradorRepository;
        _admPasswordHasher = admPasswordHasher;
    }

    public async Task<AdministradorAutenticadoDto?> LoginAdministrador(LoginDto loginDto)
    {
        var administrador = await _administradorRepository.ObterPorEmail(loginDto.Email);
        if (administrador == null)
        {
            Notificator.HandleNotFoundResource();
            return null;
        }

        var result = _admPasswordHasher.VerifyHashedPassword(administrador, administrador.Senha, loginDto.Senha);
        if (result != PasswordVerificationResult.Failed)
        {
            return new AdministradorAutenticadoDto
            {
                Id = administrador.Id,
                Email = administrador.Email,
                Nome = administrador.Nome,
                Token = await CreateToken(administrador)
            };
        }
        
        Notificator.Handle("Combinação de email e senha incorreta!");
        return null;
    }

    public Task<string> CreateToken(Administrador administrador)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(Settings.Settings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, administrador.Id.ToString()),
                new Claim(ClaimTypes.Name, administrador.Nome),
                new Claim(ClaimTypes.Email, administrador.Email),
                new Claim("TipoUsuario", ETipoUsuario.Comum.ToDescriptionString())
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return Task.FromResult(tokenHandler.WriteToken(token));
    }
}