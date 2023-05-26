using Microsoft.AspNetCore.Mvc;

namespace Cassino.Api.Controllers.Usuario;

public class UsuarioCarteira
{
    // [HttpPost]
    // public IActionResult PagarComPIX([FromBody] DadosPagamentoPIX dadosPagamento)
    // {
    //     // Crie uma instância do cliente do Pagar.me usando suas credenciais de API
    //     PagarMeService.DefaultApiKey = "sua-chave-de-api";
    //
    //     // Crie uma transação usando o Pagar.me
    //     var transaction = new PagarMeService().Transaction.New();
    //     transaction.PaymentMethod = PaymentMethod.Pix;
    //     transaction.Amount = dadosPagamento.Valor;
    //     // Defina outras propriedades da transação, se necessário
    //
    //     // Realize a transação
    //     transaction.Save();
    //
    //     // Retorne a resposta adequada para o cliente
    //     return Ok(transaction);
    // }
}