using Budgetly.Domain.Common;

namespace Budgetly.Domain.Entities;

public class BudgetHistory : AuditableEntity
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public double TargetExpense { get; set; }
    public double ActualExpense { get; set; }
    public double ActualIncome { get; set; }
    public DateTimeOffset Date { get; set; }
}