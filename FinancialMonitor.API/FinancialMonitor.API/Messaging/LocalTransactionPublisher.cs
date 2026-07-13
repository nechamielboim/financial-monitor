using FinancialMonitor.API.Hubs;
using Microsoft.AspNetCore.SignalR;
using FinancialMonitor.API.Models;
namespace FinancialMonitor.API.Messaging;

public class LocalTransactionPublisher : ITransactionPublisher
{
    private readonly IHubContext<TransactionHub> _hub;

    public LocalTransactionPublisher(IHubContext<TransactionHub> hub)
    {
        _hub = hub;
    }

    public async Task PublishAsync(Transaction transaction)
    {
        await _hub.Clients.All.SendAsync("TransactionCreated", transaction);
    }
}