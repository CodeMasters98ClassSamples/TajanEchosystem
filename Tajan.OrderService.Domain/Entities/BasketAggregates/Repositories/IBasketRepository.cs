using System.Threading.Tasks;
using Tajan.OrderService.Domain.Entities.BasketAggregates;

namespace Tajan.OrderService.Domain.Entities.BasketAggregates.Repositories;

public interface IBasketRepository
{
    Task<Basket?> GetByUserIdAsync(int userId);
    Task AddOrUpdateItemAsync(int userId, int productId, int quantity, decimal unitPrice);
    Task RemoveItemAsync(int userId, int productId);
    Task ClearBasketAsync(int userId);
}
