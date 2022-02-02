using Budgetly.Application.Common.Interfaces;
using Budgetly.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Budgetly.Application.BudgetHistory.Commands.ArchivePreviousMonthBudgets;

public class ArchivePreviousMonthBudgetsCommandHandler : IRequestHandler<ArchivePreviousMonthBudgetsCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IDateTimeService _dateTime;

    public ArchivePreviousMonthBudgetsCommandHandler(IApplicationDbContext context, IDateTimeService dateTime)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _dateTime = dateTime ?? throw new ArgumentNullException(nameof(dateTime));
    }

    public async Task<Unit> Handle(ArchivePreviousMonthBudgetsCommand request, CancellationToken cancellationToken)
    {
        var userIds = await _context.Budgets
            .Select(x => x.UserId)
            .ToListAsync(cancellationToken);

        IList<Task> taskList = userIds.Select(ArchiveBudgetByUserId).ToList();
        await Task.WhenAll(taskList);
        
        return Unit.Value;
    }

    private async Task ArchiveBudgetByUserId(string userId)
    {
        var budgetHistory = new Domain.Entities.BudgetHistory();
        CancellationToken cancellationToken = new();

        budgetHistory.UserId = userId;
        budgetHistory.Date = _dateTime.LastDayOfLastMonth;

        budgetHistory.ActualExpense = await _context.Budgets
            .Where(x => x.UserId == userId)
            .Select(x => x.ActualExpense)
            .SumAsync(cancellationToken);
           
        budgetHistory.TargetExpense = await _context.Budgets
            .Select(x => x.TargetExpense)
            .SumAsync(cancellationToken);

        budgetHistory.ActualIncome = await _context.Transactions
            .Where(x => x.UserId == userId && x.Type == TransactionTypes.Income)
            .Where(x => x.DateTime >= _dateTime.FirstDayOfLastMonth 
                        && x.DateTime <= _dateTime.LastDayOfLastMonth)
            .Select(x => x.Amount)
            .SumAsync(cancellationToken);

        await _context.BudgetHistories.AddAsync(budgetHistory, cancellationToken);

        var budgets = _context.Budgets.Where(x => x.UserId == userId);

        await budgets.ForEachAsync(x => x.ActualExpense = 0, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }
}