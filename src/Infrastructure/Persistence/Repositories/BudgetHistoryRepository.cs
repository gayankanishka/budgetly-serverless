using Budgetly.Application.Common.Interfaces;
using Budgetly.Domain.Entities;

namespace Budgetly.Infrastructure.Persistence.Repositories;

public class BudgetHistoryRepository : IBudgetHistoryRepository
{
    private readonly IApplicationDbContext _context;
    private readonly IDateTimeService _dateTimeService;

    public BudgetHistoryRepository(IApplicationDbContext context, IDateTimeService dateTimeService)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _dateTimeService = dateTimeService ?? throw new ArgumentNullException(nameof(dateTimeService));
    }

    public IQueryable<BudgetHistory> GetAll()
    {
        return _context.BudgetHistories;
    }

    public async Task AddAsync(BudgetHistory budgetHistory, CancellationToken cancellationToken = new())
    {
        budgetHistory.Date = _dateTimeService.LastDayOfPreviousMonth;

        await _context.BudgetHistories.AddAsync(budgetHistory, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}