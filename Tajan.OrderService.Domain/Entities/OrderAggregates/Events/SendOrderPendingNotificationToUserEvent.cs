using Tajan.OrderService.Domain.Abstractions;

namespace Tajan.OrderService.Domain.Entities.OrderAggregates.Events;

public class SendOrderPendingNotificationToUserEvent(int UserId) : IDomainEvent
{
    public static string Message = "کاربر گرامی سفارش شما با موفقیت ثبت شد.";

    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}
