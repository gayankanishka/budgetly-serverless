using Budgetly.Application.Common.Interfaces;
using Budgetly.Domain.Entities;
using Budgetly.Domain.Events;
using MediatR;

namespace Budgetly.Application.Transactions.Commands.GenerateRecurringTransactions;

public class GenerateRecurringTransactionsCommandHandler : IRequestHandler<GenerateRecurringTransactionsCommand>
{
    private readonly ITransactionRepository _repository;
    private readonly IDateTimeService _dateTimeService;

    public GenerateRecurringTransactionsCommandHandler(ITransactionRepository repository, IDateTimeService dateTimeService)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _dateTimeService = dateTimeService ?? throw new ArgumentNullException(nameof(dateTimeService));
    }

    public async Task<Unit> Handle(GenerateRecurringTransactionsCommand request, CancellationToken cancellationToken)
    {
        var userIds = await _repository.GetUserIds(cancellationToken);
        
        IList<Task> taskList = userIds.Select(GenerateTransactions).ToList();
        await Task.WhenAll(taskList);
        
        return Unit.Value;
    }

    private async Task GenerateTransactions(string userId)
    {
        CancellationToken cancellationToken = new();
        
        var recurringTransactions = await _repository.GetRecurringTransactions(userId, cancellationToken);

        foreach (var recurringTransaction in recurringTransactions)
        {
            Transaction transaction = new Transaction()
            {
                Name = recurringTransaction.Name,
                UserId = userId,
                Amount = recurringTransaction.Amount,
                Type = recurringTransaction.Type,
                DateTime = _dateTimeService.UtcNow,
                Note = recurringTransaction.Note,
                CategoryId = recurringTransaction.CategoryId,
                IsRecurring = recurringTransaction.IsRecurring,
                BudgetId = recurringTransaction.BudgetId
            };

            recurringTransaction.IsRecurring = false;
            await _repository.UpdateAsync(recurringTransaction, cancellationToken);

            transaction.DomainEvents.Add(new TransactionCreatedEvent(transaction));
            await _repository.AddAsync(transaction, cancellationToken);
        }
    }
}