using Budgetly.Application.Common.Interfaces;
using Budgetly.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Budgetly.Infrastructure.Persistence.Repositories;

internal sealed class BudgetRepository : IBudgetRepository
{
    private readonly IApplicationDbContext _context;
    
    public BudgetRepository(IApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public IQueryable<Budget> GetAll()
    {
        return _context.Budgets;
    }
    
    public async Task<double> GetTargetExpenseByUserIdAsync(string userId, CancellationToken cancellationToken)
    {
        return await GetAll()
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .Select(x => x.TargetExpense)
            .SumAsync(cancellationToken);
    }
    
    public async Task<double> GetActualExpenseByUserIdAsync(string userId, CancellationToken cancellationToken)
    {
        return await GetAll()
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .Select(x => x.ActualExpense)
            .SumAsync(cancellationToken);
    }

    public async Task RestBudgetByUserId(string userId, CancellationToken cancellationToken)
    {
        var budgets = _context.Budgets.Where(x => x.UserId == userId);
        await budgets.ForEachAsync(x => x.ActualExpense = 0, cancellationToken);
        
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<string>> GetUserIds(CancellationToken cancellationToken)
    {
        return await GetAll()
            .AsNoTracking()
            .Select(x => x.UserId)
            .Distinct()
            .ToListAsync(cancellationToken);
    }
    
    public async Task UpdateAsync(Budget entity, CancellationToken cancellationToken)
    {
        _context.Budgets.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}