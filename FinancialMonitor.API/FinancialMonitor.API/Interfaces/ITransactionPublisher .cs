using FinancialMonitor.API.Models;
namespace FinancialMonitor.API.Messaging;

public interface ITransactionPublisher
{
    Task PublishAsync(Transaction transaction);
}