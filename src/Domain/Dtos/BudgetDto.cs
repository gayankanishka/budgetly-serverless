namespace Budgetly.Domain.Dtos;

public class BudgetDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double TargetExpense { get; set; }
    public double ActualExpense { get; set; }
    public string? Description { get; set; }
    public int TransactionCategoryId { get; set; }
    public TransactionCategoryDto? TransactionCategory { get; set; }
}