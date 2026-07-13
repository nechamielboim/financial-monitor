using FinancialMonitor.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace FinancialMonitor.API.Data;

public class AppDbContext : DbContext
{
    public DbSet<Transaction> Transactions { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
}