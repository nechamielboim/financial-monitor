using FinancialMonitor.API.Models;

namespace FinancialMonitor.API.Interfaces;

public interface ITransactionService
{
    Task<List<Transaction>> GetAllAsync();

    Task<Transaction?> GetByIdAsync(Guid id);

    Task<Transaction> AddAsync(Transaction transaction);
}