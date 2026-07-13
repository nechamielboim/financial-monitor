using FinancialMonitor.API.Hubs;
using FinancialMonitor.API.Models;
using Microsoft.AspNetCore.SignalR;

namespace FinancialMonitor.API.Messaging;

public class RedisTransactionPublisher : ITransactionPublisher
{
    private readonly IHubContext<TransactionHub> _hubContext;

    public RedisTransactionPublisher(
        IHubContext<TransactionHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task PublishAsync(Transaction transaction)
    {
        await _hubContext.Clients.All.SendAsync(
            "TransactionCreated",
            transaction);
    }
}