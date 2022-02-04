using Budgetly.Domain.Common;
using Budgetly.Domain.Entities;

namespace Budgetly.Domain.Events;

/// <summary>
///     Event representing a new transaction being created.
/// </summary>
public class TransactionCreatedEvent : DomainEvent
{
    /// <summary>
    ///     The constructor for the event.
    /// </summary>
    /// <param name="transaction">Newly created transaction.</param>
    public TransactionCreatedEvent(Transaction transaction)
    {
        Transaction = transaction;
    }

    /// <summary>
    ///     Represents the transaction that was created.
    /// </summary>
    public Transaction Transaction { get; }
}