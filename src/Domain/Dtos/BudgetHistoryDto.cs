namespace Budgetly.Domain.Dtos;

public class BudgetHistoryDto
{
    public double TargetExpense { get; set; }
    public double ActualExpense { get; set; }
    public double ActualIncome { get; set; }
    public DateTimeOffset Date { get; set; }
}