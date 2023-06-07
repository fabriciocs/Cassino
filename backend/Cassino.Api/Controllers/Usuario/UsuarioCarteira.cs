using Cassino.Application.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

namespace Cassino.Api.Controllers.Usuario;

public class UsuarioCarteira : BaseController
{
    public UsuarioCarteira(INotificator notificator) : base(notificator)
    {
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> PagarComPix([FromBody] DadosPagamentoPixDto dto)
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
            "{\"expires_in\":120}," +
            "\"payment_method\":\"pix\"," +
            "\"amount\":" + dto.Valor + "}]," +
            "\"closed\":false}", ParameterType.RequestBody);
        RestResponse response = await client.ExecuteAsync(request);
        
        object? output = JsonConvert.DeserializeObject(response.Content);
        // Defina outras propriedades da transação, se necessário
        //response.IsSuccessStatusCode TODO: adicionar validação
        int indiceInicio = 1961; //2010; // 900 a 920 pega o status
        int comprimento = 420; // Comprimento da seção desejada

        string secaoExtraida = response.Content.Substring(indiceInicio, comprimento);

        // Retorne a resposta adequada para o cliente
        return Ok(secaoExtraida);
    }
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> UltimosPagamentos()
    {
        var client = new RestClient("https://api.pagar.me/core/v5/orders");
        var request = new RestRequest();
        request.Method = Method.Get;
        request.AddHeader("accept", "application/json");
        request.AddHeader("content-type", "application/json");
        request.AddHeader("authorization", "Basic c2tfTlE5V3FNa1RRaWFiSk0ycjo=");
        RestResponse response = await client.ExecuteAsync(request);
        return Ok(response.Content);
    }
}


public class DadosPagamentoPixDto
{
    public decimal Valor { get; set; }
}