namespace Budgetly.Application.Common.Interfaces;

/// <summary>
///     Provides the datetime required by the application.
/// </summary>
public interface IDateTimeService
{
    /// <summary>
    ///     Datetime offset of the current UTC time.
    /// </summary>
    DateTimeOffset UtcNow { get; }

    /// <summary>
    ///     First day of the current month.
    /// </summary>
    DateTimeOffset FirstDayOfCurrentMonth { get; }

    /// <summary>
    ///     Last day of the current month.
    /// </summary>
    DateTimeOffset LastDayOfCurrentMonth { get; }

    /// <summary>
    ///     One year ago from now.
    /// </summary>
    DateTimeOffset OneYearAgoFromNow { get; }
    
    /// <summary>
    ///     First day of previous month.
    /// </summary>
    public DateTimeOffset FirstDayOfPreviousMonth { get; }
    
    /// <summary>
    ///     Last day of previous month.
    /// </summary>
    public DateTimeOffset LastDayOfPreviousMonth{ get; }
}