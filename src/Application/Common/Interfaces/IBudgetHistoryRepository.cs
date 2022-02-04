namespace Budgetly.Application.Common.Interfaces;

public interface IBudgetHistoryRepository
{
    IQueryable<Domain.Entities.BudgetHistory> GetAll();
    Task AddAsync(Domain.Entities.BudgetHistory budgetHistory, CancellationToken cancellationToken = new());
}