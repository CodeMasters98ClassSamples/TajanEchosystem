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
        var basket = await GetByUserIdAsync(userId) ?? new Basket();
        // naive implementation: create basket if null
        if (basket.Items == null)
            basket = new Basket();

        var existing = basket.Items.FirstOrDefault(i => i.ProductId == productId);
        if (existing != null)
        {
            // update quantity by replacing (domain methods can be improved)
            // Note: Basket.UpdateItem implementation is placeholder in domain
            basket.DeleteItem(existing);
        }

        var item = new BasketItem();
        // reflection: set properties via EF mapping or constructor in full implementation
        // for now use public setters if available; this is a minimal scaffold

        // attach and save
        _db.BasketItems.Add(item);
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
