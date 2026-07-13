using FinancialMonitor.API.Data;
using FinancialMonitor.API.Models;
using FinancialMonitor.API.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FinancialMonitor.Tests.Services;

public class TransactionServiceTests
{
    // --------------------
    // AddAsync
    // --------------------

    [Fact]
    public async Task AddAsync_Should_Save_Transaction()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new AppDbContext(options);

        var service = new TransactionService(context);

        var transaction = new Transaction
        {
            TransactionId = Guid.NewGuid(),
            Description = "Coffee",
            Amount = 15,
            Status = TransactionStatus.Pending,
            Timestamp = DateTime.UtcNow
        };

        await service.AddAsync(transaction);

        Assert.Single(context.Transactions);

        var saved = await context.Transactions.FirstAsync();

        Assert.Equal("Coffee", saved.Description);
        Assert.Equal(15, saved.Amount);
        Assert.Equal(TransactionStatus.Pending, saved.Status);
    }

    [Fact]
    public async Task AddAsync_ShouldSaveCorrectValues()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new AppDbContext(options);

        var service = new TransactionService(context);

        var transaction = new Transaction
        {
            TransactionId = Guid.NewGuid(),
            Description = "Phone",
            Amount = 3200,
            Status = TransactionStatus.Completed,
            Timestamp = DateTime.UtcNow
        };

        await service.AddAsync(transaction);

        var saved = await context.Transactions.FirstAsync();

        Assert.Equal(transaction.TransactionId, saved.TransactionId);
        Assert.Equal("Phone", saved.Description);
        Assert.Equal(3200, saved.Amount);
        Assert.Equal(TransactionStatus.Completed, saved.Status);
    }

    // --------------------
    // GetAllAsync
    // --------------------

    [Fact]
    public async Task GetAllAsync_ShouldReturnTransactions()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new AppDbContext(options);

        context.Transactions.AddRange(
            new Transaction
            {
                TransactionId = Guid.NewGuid(),
                Description = "Coffee",
                Amount = 15,
                Status = TransactionStatus.Pending,
                Timestamp = DateTime.UtcNow
            },
            new Transaction
            {
                TransactionId = Guid.NewGuid(),
                Description = "Pizza",
                Amount = 40,
                Status = TransactionStatus.Completed,
                Timestamp = DateTime.UtcNow
            });

        await context.SaveChangesAsync();

        var service = new TransactionService(context);

        var result = await service.GetAllAsync();

        Assert.Equal(2, result.Count);
    }

    // --------------------
    // GetByIdAsync
    // --------------------

    [Fact]
    public async Task GetByIdAsync_ShouldReturnTransaction()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new AppDbContext(options);

        var transaction = new Transaction
        {
            TransactionId = Guid.NewGuid(),
            Description = "Laptop",
            Amount = 5000,
            Status = TransactionStatus.Completed,
            Timestamp = DateTime.UtcNow
        };

        context.Transactions.Add(transaction);
        await context.SaveChangesAsync();

        var service = new TransactionService(context);

        var result = await service.GetByIdAsync(transaction.TransactionId);

        Assert.NotNull(result);
        Assert.Equal(transaction.TransactionId, result!.TransactionId);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new AppDbContext(options);

        var service = new TransactionService(context);

        var result = await service.GetByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    // --------------------
    // Concurrency
    // --------------------

    [Fact]
    public async Task AddAsync_ShouldHandleConcurrentRequests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var tasks = Enumerable.Range(1, 100).Select(async i =>
        {
            using var context = new AppDbContext(options);

            var service = new TransactionService(context);

            await service.AddAsync(new Transaction
            {
                TransactionId = Guid.NewGuid(),
                Description = $"Transaction {i}",
                Amount = i,
                Status = TransactionStatus.Pending,
                Timestamp = DateTime.UtcNow
            });
        });

        await Task.WhenAll(tasks);

        using var verifyContext = new AppDbContext(options);

        var count = await verifyContext.Transactions.CountAsync();

        Assert.Equal(100, count);
    }
}