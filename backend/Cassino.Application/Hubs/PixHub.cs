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
        await Clients.Client(Context.ConnectionId).SendAsync("TransacaoPIXRecebida", dadosTransacao);
    }
    
    public async Task EnviarNotificacao(string mensagem)
    {
        // Notifique todos os clientes conectados chamando o método "ReceberNotificacao"
        await Clients.All.SendAsync("ReceberNotificacao", mensagem);
    }
}