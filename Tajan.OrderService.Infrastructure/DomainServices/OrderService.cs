using Microsoft.EntityFrameworkCore;
using Tajan.OrderService.Domain.Entities.BasketAggregates;
using Tajan.OrderService.Domain.Entities.OrderAggregates;
using Tajan.OrderService.Domain.Services;
using Tajan.Standard.Application.Abstractions;

namespace Tajan.OrderService.Infrastructure.DomainServices;

public class OrderService : IOrderService
{
    private readonly IApplicationDbContext _appDbContext;
    public OrderService(IApplicationDbContext applicationDbContext)
        => _appDbContext = applicationDbContext;
    

    public async Task SaveOrder(SaveOrderDto saveOrderDto)
    {
        var basket = await _appDbContext
            .Set<Basket>()
            .FirstOrDefaultAsync(x => x.Id == saveOrderDto.basketId);

        List<OrderDetail> details = new();
        foreach (var item in basket.Items) {
            OrderDetail detail = OrderDetail.Create(productId: 0, price: 0);
            details.Add(detail);
        }

        var order = Order.Create(description: saveOrderDto.description, userId: saveOrderDto.userId, details: details);
    }
}
