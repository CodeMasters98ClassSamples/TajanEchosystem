using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            ProductId = productId
        };
    }
    public int OrderHeaderId { get; private set; }
    public int ProductId { get; private set; }
    public Price Price { get; private set; }
}
