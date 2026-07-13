namespace FinancialMonitor.API.Models;

public class Transaction
{
    public Guid TransactionId { get; set; }

    public string Description { get; set; } = string.Empty;

    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";

    public TransactionStatus Status { get; set; }
    public DateTime Timestamp { get; set; }

}