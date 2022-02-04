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
}