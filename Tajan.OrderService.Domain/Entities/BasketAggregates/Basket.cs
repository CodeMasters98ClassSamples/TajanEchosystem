
using Tajan.OrderService.Domain.Entities.OrderAggregates.Enums;
using Tajan.Standard.Domain.Abstractions;

namespace Tajan.OrderService.Domain.Entities.BasketAggregates;

public class Basket: Entity
{
    public int Id { get; private set; }
    public string Description { get; private set; }
    public int UserId { get; private set; }
    public string Code { get; private set; }
    public Status Status { get; private set; }
    public DateTime CreateAt { get; private set; }
    public ICollection<BasketItem> Items { get; private set; }
    public Basket()
    {
        Description = string.Empty;
        Code = string.Empty;
    Status = Status.PENDING;
        CreateAt = DateTime.UtcNow;
        Items = new List<BasketItem>();
    }
    public Basket(int userId) : this()
    {
        UserId = userId;
    }
    public Basket AddItem(BasketItem item)
    {
        //
        Items.Add(item);
        return new Basket();
    }
    public void DeleteItem(BasketItem item) { Items.Remove(item); }
    public void UpdateItem(BasketItem item) { Items.Remove(item); }

}
