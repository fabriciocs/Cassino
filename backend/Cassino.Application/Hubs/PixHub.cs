using Microsoft.AspNetCore.SignalR;

namespace Cassino.Application.Hubs;

public class PixHub : Hub
{
    public async Task<string> EnviarTransacaoPix(string dadosTransacao)
    {
        await Clients.All.SendAsync("TransacaoPIXRecebida", dadosTransacao);

        return dadosTransacao;
    }
}