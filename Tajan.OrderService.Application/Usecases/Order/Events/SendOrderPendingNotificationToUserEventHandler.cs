using MassTransit;
using MassTransit.Transports;
using MediatR;
using Tajan.OrderService.Domain.Entities.OrderAggregates;
using Tajan.OrderService.Domain.Entities.OrderAggregates.Events;
using Tajan.Standard.Application.Abstractions;
using Tajan.Standard.Application.ServiceIngtegrations.NotificationService;

namespace Tajan.OrderService.Application.Usecases.Order.Events;


public class SendOrderPendingNotificationToUserEventHandler : INotificationHandler<DomainEventNotification<SendOrderPendingNotificationToUserEvent>>
{
    private readonly IPublishEndpoint _publishEndpoint;
    public SendOrderPendingNotificationToUserEventHandler(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(DomainEventNotification<SendOrderPendingNotificationToUserEvent> notification, CancellationToken ct)
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
        await _publishEndpoint.Publish<SendSingleSms>(new
        {
            
        });
    }
}