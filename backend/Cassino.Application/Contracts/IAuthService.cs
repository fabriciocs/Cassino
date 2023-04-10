using Cassino.Application.Dtos.V1.Auth;

namespace Cassino.Application.Contracts;

public interface IAuthService
{
    Task<AdministradorAutenticadoDto?> LoginAdministrador(LoginDto loginDto);
    //Task<ClienteAutenticadoDto?> Login(LoginDto loginDto);
}