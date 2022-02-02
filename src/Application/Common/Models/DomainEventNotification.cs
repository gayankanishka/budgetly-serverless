using Budgetly.Domain.Common;
using MediatR;

namespace Budgetly.Application.Common.Models;

public class DomainEventNotification<T> : INotification where T : DomainEvent
{
    public DomainEventNotification(T domainEvent)
    {
        DomainEvent = domainEvent;
    }

    public T DomainEvent { get; set; }
}