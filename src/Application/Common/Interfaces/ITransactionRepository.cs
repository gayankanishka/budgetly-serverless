using Budgetly.Domain.Entities;

namespace Budgetly.Application.Common.Interfaces;

public interface ITransactionRepository
{
    IQueryable<Transaction> GetAll();
    Task<double> GetActualIncomeAsync(string userId, DateTimeOffset startDate, DateTimeOffset endDate,
        CancellationToken cancellationToken);
}