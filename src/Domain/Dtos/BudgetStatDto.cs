namespace Budgetly.Domain.Dtos;

public class BudgetStatDto
{
    public double TargetExpense { get; set; }
    public double ActualExpense { get; set; }
    public double AvailableToSpend { get; set; }
}