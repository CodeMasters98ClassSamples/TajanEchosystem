using Microsoft.EntityFrameworkCore;
using Tajan.OrderService.Domain.Entities.BasketAggregates;
using Tajan.OrderService.Domain.Entities.BasketAggregates.Repositories;
using Tajan.OrderService.Infrastructure.Persistence.Contexts;

namespace Tajan.OrderService.Infrastructure.Persistence.Repositories;

public class BasketRepository : IBasketRepository
{
    private readonly ApplicationDbContext _db;

    public BasketRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Basket?> GetByUserIdAsync(int userId)
    {
        return await _db.Baskets.Include(b => b.Items)
            .FirstOrDefaultAsync(b => b.UserId == userId);
    }

    public async Task AddOrUpdateItemAsync(int userId, int productId, int quantity, decimal unitPrice)
    {
        var basket = await GetByUserIdAsync(userId);
        var isNew = false;
        if (basket == null)
        {
            basket = new Basket(userId);
            isNew = true;
        }

        var existing = basket.Items?.FirstOrDefault(i => i.ProductId == productId);
        if (existing != null)
        {
            // remove existing to replace with updated quantity
            basket.DeleteItem(existing);
            _db.BasketItems.Remove(existing);
        }

        var price = new Tajan.OrderService.Domain.Entities.OrderAggregates.ValueObjects.Price(unitPrice, "USD");
        var item = new BasketItem(productId, price, quantity);

    basket.Items!.Add(item);

        if (isNew)
            _db.Baskets.Add(basket);

        await _db.SaveChangesAsync();
    }

    public async Task RemoveItemAsync(int userId, int productId)
    {
        var basket = await GetByUserIdAsync(userId);
        if (basket == null) return;
        var existing = basket.Items.FirstOrDefault(i => i.ProductId == productId);
        if (existing != null)
        {
            _db.BasketItems.Remove(existing);
            await _db.SaveChangesAsync();
        }
    }

    public async Task ClearBasketAsync(int userId)
    {
        var basket = await GetByUserIdAsync(userId);
        if (basket == null) return;
        _db.BasketItems.RemoveRange(basket.Items);
        await _db.SaveChangesAsync();
    }
}
