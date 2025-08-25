using MediatR;
using Tajan.Standard.Domain.Abstractions;

namespace Tajan.Standard.Application.Abstractions;

public class DomainEventNotification<TDomainEvent>(TDomainEvent domainEvent) : INotification where TDomainEvent : IDomainEvent
{
    public TDomainEvent DomainEvent { get; } = domainEvent;
}
