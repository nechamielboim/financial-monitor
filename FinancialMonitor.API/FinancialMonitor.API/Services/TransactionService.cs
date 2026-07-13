using FinancialMonitor.API.Data;
using FinancialMonitor.API.Interfaces;
using FinancialMonitor.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FinancialMonitor.API.Services;

public class TransactionService : ITransactionService
{
    private readonly AppDbContext _context;

    public TransactionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Transaction>> GetAllAsync()
    {
        return await _context.Transactions.ToListAsync();
    }

    public async Task<Transaction?> GetByIdAsync(Guid id)
    {
        return await _context.Transactions
            .FirstOrDefaultAsync(t => t.TransactionId == id);
    }

    public async Task<Transaction> AddAsync(Transaction transaction)
    {
        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();

        return transaction;
    }
}