namespace Budgetly.Domain.Entities;

public class TransactionCategory : BaseEntity
{
    public string? Description { get; set; }
    public bool IsPreset { get; set; }
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public Budget? Budget { get; set; }
}