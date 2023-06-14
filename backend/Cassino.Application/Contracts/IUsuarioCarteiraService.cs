using Cassino.Application.Dtos.V1.Pagamentos;
using Cassino.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Cassino.Application.Contracts;

public interface IUsuarioCarteiraService
{
    Task<PixDto?> Deposito(DadosPagamentoPixDto dto);
    Task<Pagamento?> WebhookPix(HttpRequest dto);
}