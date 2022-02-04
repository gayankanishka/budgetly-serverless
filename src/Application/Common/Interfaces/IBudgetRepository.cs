using Budgetly.Domain.Entities;

namespace Budgetly.Application.Common.Interfaces;

public interface IBudgetRepository
{
    IQueryable<Budget> GetAll();
    Task<double> GetTargetExpenseByUserIdAsync(string userId, CancellationToken cancellationToken);
    Task<double> GetActualExpenseByUserIdAsync(string userId, CancellationToken cancellationToken);
    Task RestBudgetByUserId(string userId, CancellationToken cancellationToken);
    Task<IEnumerable<string>> GetUserIds(CancellationToken cancellationToken);
    Task UpdateAsync(Budget entity, CancellationToken cancellationToken);
}