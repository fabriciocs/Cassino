using Cassino.Application.Dtos.V1.Auth;

namespace Cassino.Application.Contracts;

public interface IUsuarioAuthService
{
    Task<UsuarioAutenticadoDto?> Login(LoginDto loginDto);
}