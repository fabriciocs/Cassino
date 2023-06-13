using Cassino.Application.Dtos.V1.Pagamentos;
using Microsoft.AspNetCore.Http;

namespace Cassino.Application.Contracts;

public interface IUsuarioCarteiraService
{
    Task<PixDto?> Deposito(DadosPagamentoPixDto dto);
    Task WebhookPix(HttpRequest dto);
}