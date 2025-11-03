using Tajan.OrderService.Domain.Entities.OrderAggregates.ValueObjects;
using Tajan.Standard.Domain.Abstractions;

namespace Tajan.OrderService.Domain.Entities.BasketAggregates;

public class BasketItem: Entity
{
    public int Id { get; private set; }
    public int OrderHeaderId { get; private set; }
    public int ProductId { get; private set; }
    public Price Price { get; private set; }
    public int Quantity { get; private set; }

    public BasketItem() { }

    public BasketItem(int productId, Price price, int quantity)
    {
        ProductId = productId;
        Price = price;
        Quantity = quantity;
    }

    // domain factory / mutators can be added later; for tests and scaffolding we allow internal creation
    public void SetQuantity(int qty) => Quantity = qty;
}
