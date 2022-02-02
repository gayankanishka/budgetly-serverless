using Budgetly.Domain.Common;
using Budgetly.Domain.Common.Interfaces;
using Budgetly.Domain.Enums;

namespace Budgetly.Domain.Entities;

public class Transaction : BaseEntity, IHasDomainEvent
{
    public double Amount { get; set; }
    public TransactionTypes Type { get; set; }
    public DateTimeOffset DateTime { get; set; }
    public string? Note { get; set; }
    public int CategoryId { get; set; }
    public TransactionCategory? Category { get; set; }
    public bool IsRecurring { get; set; }
    public int? BudgetId { get; set; }
    public Budget? Budget { get; set; }
    public List<DomainEvent> DomainEvents { get; set; } = new();
}