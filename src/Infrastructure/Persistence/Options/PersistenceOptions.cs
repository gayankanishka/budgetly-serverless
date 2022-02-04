namespace Budgetly.Infrastructure.Persistence.Options;

public class PersistenceOptions
{
    public const string Persistence = "Persistence";
    public string PostgreSqlConnectionString { get; set; } = string.Empty;
}