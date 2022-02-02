using Budgetly.Domain.Enums;

namespace Budgetly.Domain.Dtos;

public class TransactionDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public double Amount { get; set; }
    public TransactionTypes Type { get; set; }
    public DateTimeOffset DateTime { get; set; }
    public string? Note { get; set; }
    public TransactionCategoryDto? Category { get; set; }
    public bool IsRecurring { get; set; }
}