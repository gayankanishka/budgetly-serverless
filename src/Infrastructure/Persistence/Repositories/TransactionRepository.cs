using Budgetly.Application.Common.Interfaces;
using Budgetly.Domain.Entities;
using Budgetly.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Budgetly.Infrastructure.Persistence.Repositories;

internal sealed class TransactionRepository : ITransactionRepository
{
    private readonly IApplicationDbContext _context;

    public TransactionRepository(IApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    public IQueryable<Transaction> GetAll()
    {
        return _context.Transactions;
    }

    public async Task<double> GetActualIncomeAsync(string userId, DateTimeOffset startDate, DateTimeOffset endDate,
        CancellationToken cancellationToken)
    {
        return await GetAll()
            .Where(x => x.Type == TransactionTypes.Income && x.UserId == userId)
            .Where(x => x.DateTime >= startDate && x.DateTime <= endDate)
            .AsNoTracking()
            .Select(x => x.Amount)
            .SumAsync(cancellationToken);
    }
    
    public async Task<IEnumerable<string>> GetUserIds(CancellationToken cancellationToken)
    {
        return await GetAll()
            .AsNoTracking()
            .Select(x => x.UserId)
            .Distinct()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Transaction>> GetRecurringTransactions(string userId, CancellationToken cancellationToken)
    {
        return await GetAll()
            .AsNoTracking()
            .Where(x => x.IsRecurring && x.UserId == userId)
            .ToListAsync(cancellationToken);
    }
    
    public async Task AddAsync(Transaction entity, CancellationToken cancellationToken)
    {
        await _context.Transactions.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Transaction entity, CancellationToken cancellationToken)
    {
        _context.Transactions.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}