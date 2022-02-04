using Budgetly.Application.Common.Interfaces;
using MediatR;

namespace Budgetly.Application.BudgetHistory.Commands.ArchivePreviousMonthBudgets;

public class ArchivePreviousMonthBudgetsCommandHandler : IRequestHandler<ArchivePreviousMonthBudgetsCommand>
{
    private readonly IDateTimeService _dateTime;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IBudgetRepository _budgetRepository;
    private readonly IBudgetHistoryRepository _budgetHistoryRepository;

    public ArchivePreviousMonthBudgetsCommandHandler(IDateTimeService dateTime,
        IBudgetHistoryRepository budgetHistoryRepository,
        IBudgetRepository budgetRepository,
        ITransactionRepository transactionRepository)
    {
        _dateTime = dateTime ?? throw new ArgumentNullException(nameof(dateTime));
        _budgetHistoryRepository = budgetHistoryRepository ?? throw new ArgumentNullException(nameof(budgetHistoryRepository));
        _budgetRepository = budgetRepository ?? throw new ArgumentNullException(nameof(budgetRepository));
        _transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
    }

    public async Task<Unit> Handle(ArchivePreviousMonthBudgetsCommand request, CancellationToken cancellationToken)
    {
        var userIds = await _budgetRepository.GetUserIds(cancellationToken);

        IList<Task> taskList = userIds.Select(ArchiveBudgetByUserId).ToList();
        await Task.WhenAll(taskList);
        
        return Unit.Value;
    }

    private async Task ArchiveBudgetByUserId(string userId)
    {
        var budgetHistory = new Domain.Entities.BudgetHistory();
        CancellationToken cancellationToken = new();

        budgetHistory.UserId = userId;
        budgetHistory.ActualExpense = await _budgetRepository.GetActualExpenseByUserIdAsync(userId, cancellationToken);
        budgetHistory.TargetExpense = await _budgetRepository.GetTargetExpenseByUserIdAsync(userId, cancellationToken);
        budgetHistory.ActualIncome = await _transactionRepository.GetActualIncomeAsync(userId, _dateTime.FirstDayOfPreviousMonth,
            _dateTime.LastDayOfPreviousMonth, cancellationToken);

        await _budgetHistoryRepository.AddAsync(budgetHistory, cancellationToken);
        await _budgetRepository.RestBudgetByUserId(userId, cancellationToken);
    }
}