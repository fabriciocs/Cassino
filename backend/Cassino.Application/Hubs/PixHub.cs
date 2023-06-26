using Microsoft.AspNetCore.SignalR;

namespace Cassino.Application.Hubs;

public class PixHub : Hub
{
    
    public override Task OnConnectedAsync()
    {
        return Clients.All.SendAsync("broadcastMessage", "_SYSTEM_", $"{Context.ConnectionId}");
    }
    public async Task EnviarTransacaoPix(string dadosTransacao)
    {
        await Clients.All.SendAsync("TransacaoPIXRecebida", dadosTransacao);
    }
}