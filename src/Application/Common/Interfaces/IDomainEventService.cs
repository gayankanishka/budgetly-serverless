using Budgetly.Domain.Common;

namespace Budgetly.Application.Common.Interfaces;

public interface IDomainEventService
{
    Task Publish(DomainEvent domainEvent);
}