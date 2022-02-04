using Budgetly.Application.Common.Interfaces;
using Budgetly.Application.Common.Models;
using Budgetly.Domain.Enums;
using Budgetly.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Budgetly.Application.Transactions.EventHandlers;

public class TransactionCreatedEventHandler : INotificationHandler<DomainEventNotification<TransactionCreatedEvent>>
{
    private readonly ILogger<TransactionCreatedEventHandler> _logger;
    private readonly IBudgetRepository _repository;

    public TransactionCreatedEventHandler(ILogger<TransactionCreatedEventHandler> logger,
        IBudgetRepository repository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task Handle(DomainEventNotification<TransactionCreatedEvent> notification,
        CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;
        var transaction = domainEvent.Transaction;

        if (transaction.Type == TransactionTypes.Income)
        {
            return;
        }

        var budget = await _repository.GetAll()
            .Include(x => x.Transactions)
            .Where(x => x.TransactionCategoryId == transaction.CategoryId)
            .FirstOrDefaultAsync(cancellationToken);

        if (budget == null)
        {
            return;
        }

        budget.Transactions.Add(transaction);
        budget.ActualExpense = budget.Transactions.Sum(x => x.Amount);

        await _repository.UpdateAsync(budget, cancellationToken);

        _logger.LogInformation("----- Budgetly Serverless Domain Event: {DomainEvent} updated budget " +
                               " {BudgetId}", domainEvent.GetType().Name, budget.Id);
    }
}