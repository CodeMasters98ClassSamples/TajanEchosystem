using MediatR;
using Microsoft.EntityFrameworkCore;
using Tajan.OrderService.Domain.Entities.OrderAggregates;
using Tajan.Standard.Domain.Abstractions;
using Tajan.Standard.Infrastructure.Persistence.Contexts;

namespace Tajan.OrderService.Infrastructure.Persistence.Contexts;

public class ApplicationDbContext : SharedDbContext
{
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();



}
