using MediatR;
using Microsoft.EntityFrameworkCore;
using Tajan.OrderService.Domain.Entities.OrderAggregates;
using Tajan.Standard.Domain.Abstractions;
using Tajan.Standard.Infrastructure.Persistence.Contexts;

namespace Tajan.OrderService.Infrastructure.Persistence.Contexts;

public class ApplicationDbContext : SharedDbContext
{
    public ApplicationDbContext(DbContextOptions options, IMediator mediator) : base(options, mediator)
    {
    }

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();
    public DbSet<Tajan.OrderService.Domain.Entities.BasketAggregates.Basket> Baskets => Set<Tajan.OrderService.Domain.Entities.BasketAggregates.Basket>();
    public DbSet<Tajan.OrderService.Domain.Entities.BasketAggregates.BasketItem> BasketItems => Set<Tajan.OrderService.Domain.Entities.BasketAggregates.BasketItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Treat Price as an owned/value object of BasketItem
        modelBuilder.Entity<Tajan.OrderService.Domain.Entities.BasketAggregates.BasketItem>()
            .OwnsOne(typeof(Tajan.OrderService.Domain.Entities.OrderAggregates.ValueObjects.Price), "Price");
    }


}
