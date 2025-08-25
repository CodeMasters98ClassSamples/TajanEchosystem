using MediatR;
using Tajan.OrderService.Domain.Entities.OrderAggregates;
using Tajan.OrderService.Domain.Entities.OrderAggregates.Events;
using Tajan.Standard.Application.Abstractions;

namespace Tajan.OrderService.Application.Usecases.Order.Events;


public class SendOrderPendingNotificationToUserEventHandler : INotificationHandler<DomainEventNotification<SendOrderPendingNotificationToUserEvent>>
{
    public Task Handle(DomainEventNotification<SendOrderPendingNotificationToUserEvent> notification, CancellationToken ct)
    {
        List<OrderDetail> details = new();
        decimal totalOrderAmount = 0;
        for (int i = 0; i < details.Count; i++)
        {
            totalOrderAmount += details[i].Price.Amount;
        }
        if (totalOrderAmount > 1000)
        {
            //Send Notifictaiuon
        }

        // e.g., log, send email, enqueue outbox message, etc.
        Console.WriteLine($"[DomainEvent] Order created: {notification.DomainEvent.OccurredOnUtc}");
        return Task.CompletedTask;
    }
}