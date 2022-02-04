using Budgetly.Application.Common.Interfaces;

namespace Budgetly.Infrastructure.Services;

/// <summary>
///     Provides the datetime required by the application.
/// </summary>
public class DateTimeService : IDateTimeService
{
    /// <summary>
    ///     Datetime offset of the current UTC time.
    /// </summary>
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;

    /// <summary>
    ///     First day of the current month.
    /// </summary>
    public DateTimeOffset FirstDayOfCurrentMonth => new DateTime(UtcNow.Year, UtcNow.Month, 1);

    /// <summary>
    ///     Last day of the current month.
    /// </summary>
    public DateTimeOffset LastDayOfCurrentMonth => new DateTime(UtcNow.Year, UtcNow.Month,
        DateTime.DaysInMonth(UtcNow.Year, UtcNow.Month));

    /// <summary>
    ///     One year ago from now.
    /// </summary>
    public DateTimeOffset OneYearAgoFromNow => UtcNow.AddYears(-1);
    
    /// <summary>
    ///     First day of previous month.
    /// </summary>
    public DateTimeOffset FirstDayOfPreviousMonth => FirstDayOfCurrentMonth.AddMonths(-1);
    
    /// <summary>
    ///     Last day of previous month.
    /// </summary>
    public DateTimeOffset LastDayOfPreviousMonth =>  LastDayOfCurrentMonth.AddMonths(-1);
}