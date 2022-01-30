using Microsoft.EntityFrameworkCore;

namespace Budgetly.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext() : base()
    {
        
    }
}