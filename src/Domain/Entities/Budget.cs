using Budgetly.Domain.Common;
using Budgetly.Domain.Common.Interfaces;

namespace Budgetly.Domain.Entities;

public class Budget : BaseEntity, IHasDomainEvent
{
    public double TargetExpense { get; set; }
    public double ActualExpense { get; set; }
    public string? Description { get; set; }
    public int TransactionCategoryId { get; set; }
    public TransactionCategory? TransactionCategory { get; set; }
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public List<DomainEvent> DomainEvents { get; set; } = new();
}