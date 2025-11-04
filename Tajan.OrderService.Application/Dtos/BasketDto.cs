using System.Collections.Generic;

namespace Tajan.OrderService.Application.Dtos;

public class BasketItemDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineTotal { get; set; }
}

public class BasketDto
{
    public int UserId { get; set; }
    public List<BasketItemDto> Items { get; set; } = new List<BasketItemDto>();
    public decimal Subtotal { get; set; }
}
