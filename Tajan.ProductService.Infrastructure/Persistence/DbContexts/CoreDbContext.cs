using MediatR;
using Microsoft.EntityFrameworkCore;
using Tajan.ProductService.API.Entities;
using Tajan.Standard.Infrastructure.Persistence.Contexts;

namespace Tajan.ProductService.Infrastructure.DbContexts;

public class CoreDbContext : SharedDbContext
{
    public CoreDbContext(DbContextOptions<CoreDbContext> options,IMediator mediator) : base(options, mediator)
    {
    }

    public DbSet<Product> Products { get; set; }

}