using FinancialMonitor.API.Data;
using Microsoft.EntityFrameworkCore;
using FinancialMonitor.API.Interfaces;
using FinancialMonitor.API.Services;
using FinancialMonitor.API.Apis;
using FinancialMonitor.API.Hubs;
using FinancialMonitor.API.Messaging;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();
//builder.Services.AddSignalR()
//    .AddStackExchangeRedis("localhost:6379");
builder.Services.AddSingleton<ITransactionPublisher,LocalTransactionPublisher >();
builder.Services.AddCors(options =>
{
    options.AddPolicy("React", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=financialmonitor.db"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("React");
app.UseAuthorization();
app.MapHub<TransactionHub>("/hubs/transactions");

app.MapControllers();
app.MapTransactionsApi();
app.Run();
