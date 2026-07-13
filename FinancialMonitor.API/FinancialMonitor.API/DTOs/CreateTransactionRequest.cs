using FinancialMonitor.API.Models;

namespace FinancialMonitor.API.DTOs;

public record CreateTransactionRequest(
    string Description,
    decimal Amount
)
{
    public Transaction ToTransaction()
    {
        return new Transaction
        {
            TransactionId = Guid.NewGuid(),
            Description = Description,
            Amount = Amount,
            Currency = "USD",
            Status = TransactionStatus.Pending,
            Timestamp = DateTime.UtcNow
        };
    }
}