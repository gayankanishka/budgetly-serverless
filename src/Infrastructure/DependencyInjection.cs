using Budgetly.Application.Common.Interfaces;
using Budgetly.Infrastructure.Persistence;
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
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql("Host=kubernetes.docker.internal;Database=BudgetlyDb;Username=postgres;Password=1qaz2wsx@",
                a =>
                    a.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        services.AddTransient<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>() 
                                                              ?? throw new InvalidOperationException());
        services.AddScoped<IDomainEventService, DomainEventService>();
        
        services.AddTransient<IDateTimeService, DateTimeService>();
        
        return services;
    }
}