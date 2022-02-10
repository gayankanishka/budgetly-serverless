using Budgetly.Domain.Entities;

namespace Budgetly.Application.Common.Interfaces;

public interface ITransactionRepository
{
    IQueryable<Transaction> GetAll();
    Task<double> GetActualIncomeAsync(string userId, DateTimeOffset startDate, DateTimeOffset endDate,
        CancellationToken cancellationToken);
    Task<IEnumerable<string>> GetUserIds(CancellationToken cancellationToken);
    Task<IEnumerable<Transaction>> GetRecurringTransactions(string userId, CancellationToken cancellationToken);
    Task AddAsync(Transaction entity, CancellationToken cancellationToken);
    Task UpdateAsync(Transaction entity, CancellationToken cancellationToken);
}