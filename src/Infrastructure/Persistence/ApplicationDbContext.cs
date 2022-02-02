using System.Reflection;
using Budgetly.Application.Common.Interfaces;
using Budgetly.Domain.Common;
using Budgetly.Domain.Common.Interfaces;
using Budgetly.Domain.Entities;
using Budgetly.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Budgetly.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly IDateTimeService _dateTimeService;
    private readonly IDomainEventService _domainEventService;
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IDateTimeService dateTimeService, 
        IDomainEventService domainEventService)
        : base(options)
    {
        if (options == null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        _dateTimeService = dateTimeService ?? throw new ArgumentNullException(nameof(dateTimeService));
        _domainEventService = domainEventService ?? throw new ArgumentNullException(nameof(domainEventService));
    }
    
    public DbSet<Budget> Budgets => Set<Budget>();
    public DbSet<TransactionCategory> TransactionCategories => Set<TransactionCategory>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<BudgetHistory> BudgetHistories => Set<BudgetHistory>();
    
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = "NightlyJob";
                    entry.Entity.Created = _dateTimeService.UtcNow;
                    entry.Entity.UserId = "NightlyJob";
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModifiedBy ="NightlyJob";
                    entry.Entity.LastModified = _dateTimeService.UtcNow;
                    break;
            }
        }

        var events = ChangeTracker.Entries<IHasDomainEvent>()
            .Select(x => x.Entity.DomainEvents)
            .SelectMany(x => x)
            .Where(domainEvent => !domainEvent.IsPublished)
            .ToArray();

        var results = await base.SaveChangesAsync(cancellationToken);

        await DispatchEvents(events);

        return results;
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        modelBuilder
            .Entity<Transaction>()
            .Property(_ => _.Type)
            .HasConversion(
                _ => _.ToString(),
                _ => (TransactionTypes)Enum.Parse(typeof(TransactionTypes), _));
        
        modelBuilder.Entity<Transaction>().Ignore(e => e.DomainEvents);
        modelBuilder.Entity<Budget>().Ignore(e => e.DomainEvents);
        base.OnModelCreating(modelBuilder);
    }

    private async Task DispatchEvents(DomainEvent[] events)
    {
        foreach (var @event in events)
        {
            @event.IsPublished = true;
            await _domainEventService.Publish(@event);
        }
    }
}