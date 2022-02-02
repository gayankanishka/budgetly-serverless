using Budgetly.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Budgetly.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Budget> Budgets { get; }
    DbSet<TransactionCategory> TransactionCategories { get; }
    DbSet<Transaction> Transactions { get; }
    DbSet<Domain.Entities.BudgetHistory> BudgetHistories { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}