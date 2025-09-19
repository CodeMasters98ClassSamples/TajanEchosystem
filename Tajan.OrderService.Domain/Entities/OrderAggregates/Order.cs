using Tajan.OrderService.Domain.Entities.OrderAggregates.Enums;
using Tajan.OrderService.Domain.Entities.OrderAggregates.Events;
using Tajan.OrderService.Domain.Entities.OrderAggregates.Exceptions;
using Tajan.Standard.Domain.Abstractions;

namespace Tajan.OrderService.Domain.Entities.OrderAggregates;

public class Order : Entity
{

    public static Order Create(string description, int userId, List<OrderDetail> details)
    {
        //Domain Business Rule
        if (details is null || details.Count == 0)
            throw new InvalidOrderDetailException();

        if (userId > 0)
            throw new InvalidUserException();

        var order = new Order()
        {
            Id = userId,
            Description = description,
            Status = Status.ACCEPTED
        };

        foreach (var detail in details)
            order.AddDetail(detail: detail);

        order.Raise(new SendOrderPendingNotificationToUserEvent(UserId: userId));

        return order;
    }

    public Order()
    {

    }

    public int Id { get; private set; }
    public string Description { get; private set; }
    public int UserId { get; private set; }
    public string Code { get; private set; }
    public Status Status { get; private set; }
    public DateTime CreateAt { get; private set; }
    public ICollection<OrderDetail> Details { get; private set; }

    public void AddDetail(OrderDetail detail)
    {
        if (Status == Status.PENDING)
            Details.Add(detail);
        else
            throw new InvalidOrderDetailException();
    }

    public void CancelOrder()
    {
        if (CreateAt <= DateTime.Now.AddHours(-5))
        {
            Status = Status.CANCELED;
        }
    }
}
