namespace Tajan.OrderService.Domain.Services;

public interface IOrderService
{
    Task SaveOrder(SaveOrderDto saveOrderDto);
}

public record SaveOrderDto(int basketId, string userId, string description);
