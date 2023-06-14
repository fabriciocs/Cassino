using AutoMapper;
using Cassino.Application.Contracts;
using Cassino.Application.Dtos.V1.Pagamentos;
using Cassino.Application.Notification;
using Cassino.Domain.Contracts.Repositories;
using Cassino.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using RestSharp;

namespace Cassino.Application.Services;

public class UsuarioCarteiraService : BaseService, IUsuarioCarteiraService
{
    private readonly IPagamentosRepository _repository;
    public UsuarioCarteiraService(IMapper mapper, INotificator notificator, IPagamentosRepository repository) : base(mapper, notificator)
    {
        _repository = repository;
    }
    
    public async Task<PixDto?> Deposito(DadosPagamentoPixDto dto)
    {
        var client = new RestClient("https://api.pagar.me/core/v5/orders");
        var request = new RestRequest();
        request.Method = Method.Post;
        request.AddHeader("accept", "application/json");
        request.AddHeader("content-type", "application/json");
        request.AddHeader("authorization", "Basic c2tfTlE5V3FNa1RRaWFiSk0ycjo=");//c2tfdGVzdF8wdzFPNWx6c3pSc1hMSk1QOg==");
        request.AddParameter
        ("application/json",
            "{\"customer\":" +
            "{\"phones\":{\"mobile_phone\":{\"country_code\":\"55\",\"area_code\":\"859\",\"number\":\"92548520\"}}," +
            "\"name\":\"Cliente teste\"," +
            "\"email\":\"clienteproducao@producao.com.br\"," +
            "\"document\":\"90010076000\"," +
            "\"document_type\":\"CPF\"," +
            "\"type\":\"individual\"}," +
            "\"items\":[{" +
            "\"amount\":"+ dto.Valor +"," +
            "\"description\":\"Transferencia normal\"," +
            "\"quantity\":1}]," +
            "\"payments\":[{" +
            "\"Pix\":" +
            "{\"expires_in\":300}," +
            "\"payment_method\":\"pix\"," +
            "\"amount\":" + dto.Valor + "}]," +
            "\"closed\":false}", ParameterType.RequestBody);
        RestResponse response = await client.ExecuteAsync(request);
        if (!response.IsSuccessStatusCode || response.Content == null)
        {
            Notificator.Handle("Algo deu errado, não foi possível realizar o pagamento!");
            return null;
        }
        
        var pagarmeResponse = JsonConvert.DeserializeObject<PixResponseDto>(response.Content);
        if (pagarmeResponse is null)
        {
            Notificator.HandleNotFoundResource();
            return null;
        }
        
        var dados = pagarmeResponse.charges.FirstOrDefault();
        if (dados is null)
        {
            return null;
        }
        
        var pix = new PixDto
        {
            amount = dados.last_transaction.amount,
            qr_code = dados.last_transaction.qr_code,
            qr_code_url = dados.last_transaction.qr_code_url,
            status = dados.last_transaction.status,
            expires_at = dados.last_transaction.expires_at,
            id = pagarmeResponse.id
        };

        var pagamento = new Pagamento
        {
            Valor = dados.last_transaction.amount,
            Aprovado = false,
            PagamentoId = pagarmeResponse.id,
            UsuarioId = dto.UsuarioId,
            DataPagamento = dados.last_transaction.created_at,
            DataExpiracaoPagamento = dados.last_transaction.expires_at
        };
        
        _repository.Adicionar(pagamento);
        if (!await _repository.UnitOfWork.Commit())
        {
            Notificator.Handle("Não foi possível salvar pagamento.");
            return null;
        }
        return pix;
    }

    public async Task<Pagamento?> WebhookPix(HttpRequest dto)
    {
        using StreamReader reader = new StreamReader(dto.Body);
        string bodyContent = await reader.ReadToEndAsync();
        
        var pagarmeResponse = JsonConvert.DeserializeObject<Root>(bodyContent);
        var pagamento = await _repository.FistOrDefault(c => c.PagamentoId == pagarmeResponse.id);
        if (pagamento is null)
        {
            return null;
        }

        pagamento.Aprovado = true;
        _repository.Alterar(pagamento);
        if (!await _repository.UnitOfWork.Commit())
        {
            Notificator.Handle("Não foi possível salvar pagamento.");
        }

        return pagamento;
    }
    
    private async Task VerificarPagemnto(PixDto dto)
    {
        bool pagamentoConfirmado = false;
        // var timer = new Timer(VerificarPagemnto(), null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

        do
        {

            var client = new RestClient($"https://api.pagar.me/core/v5/orders/{dto.id}");
            var request = new RestRequest();
            request.Method = Method.Get;
            request.AddHeader("accept", "application/json");
            request.AddHeader("authorization", "Basic OnNrX05ROVdxTWtUUWlhYkpNMnI=");
            RestResponse response = await client.ExecuteAsync(request);
            var pagarmeResponse = JsonConvert.DeserializeObject<PixResponseDto>(response.Content);

            if (pagarmeResponse is null)
            {
                Notificator.HandleNotFoundResource();
                return;
            }

            var dados = pagarmeResponse.charges.FirstOrDefault();
            if (dados is null) {
                Notificator.HandleNotFoundResource();
            }

            if (dados.last_transaction.status == "Paid")
            {
                pagamentoConfirmado = true;
            }
            
            await Task.Delay(TimeSpan.FromSeconds(10));
        } while (pagamentoConfirmado != true);
    }
}