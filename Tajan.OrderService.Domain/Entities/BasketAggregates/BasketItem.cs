using Tajan.OrderService.Domain.Entities.OrderAggregates.ValueObjects;
using Tajan.Standard.Domain.Abstractions;

namespace Tajan.OrderService.Domain.Entities.BasketAggregates;

public class BasketItem: Entity
{
    public int OrderHeaderId { get; private set; }
    public int ProductId { get; private set; }
    public Price Price { get; private set; }
}
