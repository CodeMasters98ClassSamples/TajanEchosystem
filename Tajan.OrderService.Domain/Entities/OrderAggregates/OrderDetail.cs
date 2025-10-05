using Tajan.OrderService.Domain.Entities.OrderAggregates.ValueObjects;

namespace Tajan.OrderService.Domain.Entities.OrderAggregates;

public class OrderDetail
{
    public OrderDetail()
    {

    }

    public static OrderDetail Create(int productId,decimal price)
    {
        return new OrderDetail()
        {
            ProductId = productId,
            Price = price,
            //Price =  new Price(price,"RIAL")
        };
    }
    public int Id { get; set; }
    public int OrderHeaderId { get; private set; }
    public int ProductId { get; private set; }
    public bool IsDeleted { get; set; }
    public decimal Price { get; private set; }
    //public Price Price { get; private set; }
}
