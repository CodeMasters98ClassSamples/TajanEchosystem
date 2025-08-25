using System.ComponentModel;
using Tajan.OrderService.Domain.Abstractions;
using Tajan.OrderService.Domain.Entities.OrderAggregates.Enums;
using Tajan.OrderService.Domain.Entities.OrderAggregates.Events;

namespace Tajan.OrderService.Domain.Entities.OrderAggregates;

public class Order : Entity
{

    public static Order Create(string description, int userId, List<OrderDetail> details)
    {
        if (details is null || details.Count == 0)
        {
            throw new Exception();// Domain Excewption
        }

        //Domain Business Rule
        if (userId > 0)
        {
            throw new Exception();// Domain Excewption
        }

        Raise(new SendOrderPendingNotificationToUserEvent(UserId: userId));

        return new Order()
        {
            Id = userId,
            Description = description,
            Status = Status.ACCEPTED
        };

    
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

    public void AddDetail(Order order,OrderDetail detail)
    {
        if (order.Status == Status.PENDING)
        {
            order.Details.Add(detail);
        }
        else
        {
            throw new Exception();// Domain Exption
        }
    }

    public void CancelOrder(Order order)
    {
        if (order == null)
        {
        }

        if (order.CreateAt <= DateTime.Now.AddHours(-5))
        {
            order.Status = Status.CANCELED;
        }
    }
}
