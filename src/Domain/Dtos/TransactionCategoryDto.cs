namespace Budgetly.Domain.Dtos;

public class TransactionCategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool IsPreset { get; set; }
}