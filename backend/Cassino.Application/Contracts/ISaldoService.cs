using Cassino.Application.Dtos.V1.Aposta;
using Cassino.Application.Dtos.V1.Saldo;

namespace Cassino.Application.Contracts
{
    public interface ISaldoService
    {
        Task<SaldoUsuarioDto?> BuscarSaldo(int id);
        Task<SaldoUsuarioDto?> AtualizarSaldo(AdicionarApostaDto apostaDto);
        Task AtualizarSaldo(Decimal valor, int usuarioId);
    }
}
