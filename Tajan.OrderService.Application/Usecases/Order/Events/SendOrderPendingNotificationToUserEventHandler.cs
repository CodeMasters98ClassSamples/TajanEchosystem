using MassTransit;
using MediatR;
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
        await _publishEndpoint.Publish<SendSingleSms>(new
        {
            MobileNumber = "09129564205",
            Content = "سفارش شما با موفقیت انجام شد.",
            Title = "تکمیل سفارش",
        });
    }
}