using Budgetly.Application.Common.Interfaces;
using Budgetly.Domain.Entities;
using Budgetly.Infrastructure.Persistence;
using Budgetly.Infrastructure.Persistence.Options;
using Budgetly.Infrastructure.Persistence.Repositories;
using Budgetly.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Budgetly.Infrastructure;

/// <summary>
///     Dependency injection extension to configure Infrastructure layer services.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    ///     Configure Infrastructure layer services.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseOptions = configuration.GetSection(PersistenceOptions.Persistence)
            .Get<PersistenceOptions>();
        
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(databaseOptions.PostgreSqlConnectionString,
                a =>
                    a.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>() 
                                                              ?? throw new InvalidOperationException());
        services.AddScoped<IDomainEventService, DomainEventService>();
        services.AddScoped<IBudgetRepository, BudgetRepository>();
        services.AddScoped<IBudgetHistoryRepository, BudgetHistoryRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        
        services.AddTransient<IDateTimeService, DateTimeService>();
        
        return services;
    }
}