using Cassino.Application.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PagarmeCoreApi.PCL.Models;
using RestSharp;

namespace Cassino.Api.Controllers.Usuario;

public class UsuarioCarteira : BaseController
{
    public UsuarioCarteira(INotificator notificator) : base(notificator)
    {
    }
    
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> PagarComPIX([FromBody] DadosPagamentoPixDto dadosPagamento)
    {
        // var a = new CreateCustomerRequest
        // {
        //     
        // }
        // Crie uma transação usando o Pagar.me
        var test = new CreatePaymentRequest
        {
            Amount = 100,
            PaymentMethod = "pix",
            Pix = new CreatePixPaymentRequest
            {
                ExpiresIn = 120,
                ExpiresAt = DateTime.Now.AddSeconds(120)
            }
        };

        var client = new RestClient("https://api.pagar.me/core/v5/orders");
        var request = new RestRequest();
        request.Method = Method.Post;
        request.AddHeader("accept", "application/json");
        request.AddHeader("content-type", "application/json");
        request.AddHeader("authorization", "Basic c2tfdGVzdF8wdzFPNWx6c3pSc1hMSk1QOg==");
        request.AddParameter
        ("application/json",
            "{\"customer\":" +
            "{\"phones\":{\"mobile_phone\":{\"country_code\":\"55\",\"area_code\":\"859\",\"number\":\"92548520\"}}," +
            "\"name\":\"Tony Stark\"," +
            "\"email\":\"avengerstark@ligadajustica.com.br\"," +
            "\"document\":\"90010076000\"," +
            "\"document_type\":\"CPF\"," +
            "\"type\":\"individual\"}," +
            "\"items\":[{" +
            "\"amount\":100," +
            "\"description\":\"Chaveiro do Tesseract\"," +
            "\"quantity\":1}]," +
            "\"payments\":[{" +
            "\"Pix\":" +
            "{\"expires_in\":120}," +
            "\"payment_method\":\"pix\"," +
            "\"amount\":100}]," +
            "\"closed\":false}", ParameterType.RequestBody);
        RestResponse response = await client.ExecuteAsync(request);
        
        object? output = JsonConvert.DeserializeObject(response.Content);
        // Defina outras propriedades da transação, se necessário
        
        // Realize a transação

        // Retorne a resposta adequada para o cliente
        return Ok(response.Content);
    }
}

public class DadosPagamentoPixDto
{
    
}