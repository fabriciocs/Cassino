using AutoMapper;
using Cassino.Application.Contracts;
using Cassino.Application.Dtos.V1.Pagamentos;
using Cassino.Application.Notification;
using Cassino.Domain.Contracts.Repositories;
using Cassino.Domain.Entities;
using RestSharp;

namespace Cassino.Application.Services;

public class UsuarioCarteiraService : BaseService, IUsuarioCarteiraService
{
    private readonly IPagamentosRepository _repository;
    public UsuarioCarteiraService(IMapper mapper, INotificator notificator, IPagamentosRepository repository) : base(mapper, notificator)
    {
        _repository = repository;
    }
    
    public async Task<string?> Deposito(DadosPagamentoPixDto dto)
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
        
        int indiceInicio = 1961;
        int comprimento = 430;

        var pagamento = new Pagamento
        {
            Valor = dto.Valor,
            Aprovado = false,
            UsuarioId = dto.UsuarioId,
            Conteudo = response.Content,
            DataPagamento = DateTime.Now,
            DataExpiracaoPagamento = DateTime.Now.AddMinutes(5)
        };
        
        _repository.Adicionar(pagamento);
        if (!await _repository.UnitOfWork.Commit())
        {
            Notificator.Handle("Não foi possível salvar pagamento.");
            return null;
        }
        
        string secaoExtraida = response.Content.Substring(indiceInicio, comprimento);

        return secaoExtraida;
    }
}