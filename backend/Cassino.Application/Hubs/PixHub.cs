using Microsoft.AspNetCore.SignalR;

namespace Cassino.Application.Hubs;

public class PixHub : Hub
{
    public async Task EnviarTransacaoPix(string dadosTransacao)
    {
        
        await Clients.All.SendAsync("TransacaoPIXRecebida", dadosTransacao);
    }
}