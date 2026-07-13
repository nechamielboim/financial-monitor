using Microsoft.AspNetCore.SignalR;

namespace FinancialMonitor.API.Hubs;

public class TransactionHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        Console.WriteLine($"CONNECTED: {Context.ConnectionId}");

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        Console.WriteLine($"DISCONNECTED: {Context.ConnectionId}");

        await base.OnDisconnectedAsync(exception);
    }
}