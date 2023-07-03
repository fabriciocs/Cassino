using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using Cassino.Application.Contracts;
using Cassino.Application.Dtos.V1.Pagamentos;
using Cassino.Application.Hubs;
using Cassino.Application.Notification;
using Cassino.Domain.Contracts.Repositories;
using Cassino.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using RestSharp;

namespace Cassino.Application.Services;

public class UsuarioCarteiraService : BaseService, IUsuarioCarteiraService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPagamentosRepository _repository;
    private readonly ISaldoService _service;
    private readonly IHubContext<PixHub> _confirmationpix;

    public UsuarioCarteiraService(IMapper mapper, INotificator notificator, IPagamentosRepository repository,
        ISaldoService service, IUsuarioRepository usuarioRepository, IHubContext<PixHub> confirmationpix) : base(mapper,
        notificator)
    {
        _repository = repository;
        _service = service;
        _usuarioRepository = usuarioRepository;
        _confirmationpix = confirmationpix;
    }

    private async Task<string?> Autenticar()
    {
        string filePath = Path.Combine(Directory.GetCurrentDirectory(),
            "Certificates\\producao-467170-jogosProducao.p12");
        X509Certificate2 uidCert = new X509Certificate2(filePath, "");

        var options = new RestClientOptions("https://api-pix.gerencianet.com.br/oauth/token")
        {
            ClientCertificates = new X509CertificateCollection { uidCert }
        };
        var client = new RestClient(options);
        var request = new RestRequest();
        request.Method = Method.Post;
        var credencials = new Dictionary<string, string>
        {
            { "client_id", "Client_Id_e53549ce91fe086e73d44d9238d3770e2ad08035" },
            { "client_secret", "Client_Secret_1485ed3cda1cd75dde165c8578b1aa8f0c63f2c1" }
        };

        byte[] texto =
            System.Text.Encoding.UTF8.GetBytes(credencials["client_id"] + ":" + credencials["client_secret"]);
        var authorization = Convert.ToBase64String(texto);
        request.AddHeader("Authorization", "Basic " + authorization);
        request.AddHeader("Content-Type", "application/json");
        request.AddParameter("application/json", "{\r\n    \"grant_type\": \"client_credentials\"\r\n}",
            ParameterType.RequestBody);

        RestResponse response = await client.ExecuteAsync(request);
        if (!response.IsSuccessStatusCode || response.Content == null)
        {
            Notificator.Handle("Algo deu errado, não foi possível realizar o pagamento!");
            return null;
        }

        var gerenciaResponse = JsonConvert.DeserializeObject<authDto>(response.Content);

        return gerenciaResponse.access_token;
    }

    public async Task<ObterQrCodeDto?> Deposito(DadosPagamentoPixDto dto)
    {
        var client = new RestClient("https://api-pix.gerencianet.com.br/v2/cob");
        var request = new RestRequest();
        request.Method = Method.Post;
        string jsonBody = @"
            {
                ""calendario"": {
                ""expiracao"": 3600
            },
                ""devedor"": {
                ""cpf"": ""12345678909"",
                ""nome"": ""Francisco da Silva""
            },
                ""valor"": {
                ""original"": """ + dto.Valor + @"""
            },
                ""chave"": ""71cdf9ba-c695-4e3c-b010-abb521a3f1be"",
                ""solicitacaoPagador"": ""Informe o número ou identificador do pedido.""
            }";

        var token = Autenticar().ToString();
        request.AddHeader("Authorization", "Basic " + token);
        request.AddHeader("Content-Type", "application/json");
        request.AddParameter("application/json", jsonBody,
            ParameterType.RequestBody);

        RestResponse response = await client.ExecuteAsync(request);
        if (!response.IsSuccessStatusCode || response.Content == null)
        {
            Notificator.Handle("Algo deu errado, não foi possível realizar o pagamento!");
            return null;
        }

        var gerenciaResponse = JsonConvert.DeserializeObject<CriarDepositoDto>(response.Content);

        var pagamento = new Pagamento
        {
            Valor = decimal.Parse(gerenciaResponse.valor.original),
            Aprovado = false,
            PagamentoId = gerenciaResponse.loc.id.ToString(),
            UsuarioId = dto.UsuarioId,
            DataPagamento = gerenciaResponse.calendario.criacao,
            DataExpiracaoPagamento = gerenciaResponse.calendario.criacao
                .AddMilliseconds(gerenciaResponse.calendario.expiracao) 
        };

        _repository.Adicionar(pagamento);
        if (!await _repository.UnitOfWork.Commit())
        {
            Notificator.Handle("Não foi possível salvar pagamento.");
            return null;
        }
        
        // await _confirmationpix.Clients.Client(connectionId:Context).SendAsync("TransacaoPIXRecebida");
        await _confirmationpix.Clients.All.SendAsync("TransacaoPIXRecebida", $"{pagamento.UsuarioId}");
        var qrCode = await GerarQrCode(token, gerenciaResponse);
        
        return qrCode;
    }

    private async Task<ObterQrCodeDto?> GerarQrCode(string token, CriarDepositoDto dto)
    {
        var request = new RestRequest();
        request.Method = Method.Get;
        request.AddHeader("Authorization", "Basic " + token);
        var client = new RestClient($"https://api-pix.gerencianet.com.br/v2/loc/{dto.loc.id}/qrcode");

        RestResponse response = await client.ExecuteAsync(request);
        if (!response.IsSuccessStatusCode || response.Content == null)
        {
            Notificator.Handle("Algo deu errado, não foi possível realizar o pagamento!");
            return null;
        }
        
        var gerenciaResponse = JsonConvert.DeserializeObject<ObterQrCodeDto>(response.Content);

        return gerenciaResponse;
    }

    // public async Task<PixDto?> Deposito(DadosPagamentoPixDto dto)
    // {
    //     var client = new RestClient("https://api.pagar.me/core/v5/orders");
    //     var request = new RestRequest();
    //     request.Method = Method.Post;
    //     request.AddHeader("accept", "application/json");
    //     request.AddHeader("content-type", "application/json");
    //     request.AddHeader("authorization",
    //         "Basic c2tfTlE5V3FNa1RRaWFiSk0ycjo="); //c2tfdGVzdF8wdzFPNWx6c3pSc1hMSk1QOg==");
    //     request.AddParameter
    //     ("application/json",
    //         "{\"customer\":" +
    //         "{\"phones\":{\"mobile_phone\":{\"country_code\":\"55\",\"area_code\":\"859\",\"number\":\"92548520\"}}," +
    //         "\"name\":\"Cliente teste\"," +
    //         "\"email\":\"clienteproducao@producao.com.br\"," +
    //         "\"document\":\"90010076000\"," +
    //         "\"document_type\":\"CPF\"," +
    //         "\"type\":\"individual\"}," +
    //         "\"items\":[{" +
    //         "\"amount\":" + dto.Valor + "," +
    //         "\"description\":\"Transferencia normal\"," +
    //         "\"quantity\":1}]," +
    //         "\"payments\":[{" +
    //         "\"Pix\":" +
    //         "{\"expires_in\":300}," +
    //         "\"payment_method\":\"pix\"," +
    //         "\"amount\":" + dto.Valor + "}]," +
    //         "\"closed\":false}", ParameterType.RequestBody);
    //     RestResponse response = await client.ExecuteAsync(request);
    //     if (!response.IsSuccessStatusCode || response.Content == null)
    //     {
    //         Notificator.Handle("Algo deu errado, não foi possível realizar o pagamento!");
    //         return null;
    //     }
    //
    //     var pagarmeResponse = JsonConvert.DeserializeObject<PixResponseDto>(response.Content);
    //     if (pagarmeResponse is null)
    //     {
    //         Notificator.HandleNotFoundResource();
    //         return null;
    //     }
    //
    //     var dados = pagarmeResponse.charges.FirstOrDefault();
    //     if (dados is null)
    //     {
    //         return null;
    //     }
    //
    //     var pix = new PixDto
    //     {
    //         amount = dados.last_transaction.amount,
    //         qr_code = dados.last_transaction.qr_code,
    //         qr_code_url = dados.last_transaction.qr_code_url,
    //         status = dados.last_transaction.status,
    //         expires_at = dados.last_transaction.expires_at,
    //         id = pagarmeResponse.id
    //     };
    //
    //     var pagamento = new Pagamento
    //     {
    //         Valor = dados.last_transaction.amount,
    //         Aprovado = false,
    //         PagamentoId = dados.last_transaction.id,
    //         UsuarioId = dto.UsuarioId,
    //         DataPagamento = dados.last_transaction.created_at,
    //         DataExpiracaoPagamento = dados.last_transaction.expires_at
    //     };

    //     _repository.Adicionar(pagamento);
    //     if (!await _repository.UnitOfWork.Commit())
    //     {
    //         Notificator.Handle("Não foi possível salvar pagamento.");
    //         return null;
    //     }
    //
    //     // await _confirmationpix.Clients.Client(connectionId:Context).SendAsync("TransacaoPIXRecebida");
    //     await _confirmationpix.Clients.All.SendAsync("TransacaoPIXRecebida", $"{pagamento.UsuarioId}");
    //     return pix;
    // }

    public async Task<Pagamento?> WebhookPix(HttpRequest dto)
    {
        using StreamReader reader = new StreamReader(dto.Body);
        string bodyContent = await reader.ReadToEndAsync();
        var pagarmeResponse = JsonConvert.DeserializeObject<Root>(bodyContent);
        if (pagarmeResponse is null)
        {
            return null;
        }

        var pagamento = await _repository.FistOrDefault(c =>
            pagarmeResponse.data.charges.FirstOrDefault()!.last_transaction.qr_code_url.Contains(c.PagamentoId));
        if (pagamento is null)
        {
            return null;
        }

        pagamento.Aprovado = true;
        var usuario = await _usuarioRepository.ObterPorId(pagamento.UsuarioId);
        if (usuario is null)
        {
            Notificator.HandleNotFoundResource();
            return null;
        }

        await _service.AtualizarSaldo(pagarmeResponse.data.amount, pagamento.UsuarioId);
        _repository.Alterar(pagamento);
        if (!await _repository.UnitOfWork.Commit())
        {
            Notificator.Handle("Não foi possível salvar pagamento.");
        }

        await _confirmationpix.Clients.All.SendAsync("TransacaoPIXRecebida", $"{pagamento.UsuarioId}");
        return pagamento;
    }
}