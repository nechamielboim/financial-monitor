using FinancialMonitor.API.DTOs;
using FinancialMonitor.API.Interfaces;
using FinancialMonitor.API.Messaging;

namespace FinancialMonitor.API.Apis;

public static class TransactionsApi
{
    public static void MapTransactionsApi(this WebApplication app)
    {
        var group = app.MapGroup("/transactions");

        // GET /transactions
        group.MapGet("/", async (ITransactionService service) =>
        {
            var transactions = await service.GetAllAsync();

            return Results.Ok(transactions);
        });

        // GET /transactions/{id}
        group.MapGet("/{id:guid}", async (
            Guid id,
            ITransactionService service) =>
        {
            var transaction = await service.GetByIdAsync(id);

            if (transaction is null)
                return Results.NotFound();

            return Results.Ok(transaction);
        });

        // POST /transactions
        group.MapPost("/", async (
            CreateTransactionRequest request,
            ITransactionService service,
            ITransactionPublisher publisher) =>
        {
            var transaction = request.ToTransaction();

            var created = await service.AddAsync(transaction);

            await publisher.PublishAsync(created);

            return Results.Created(
                $"/transactions/{created.TransactionId}",
                created);
        });
    }
}