using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Tajan.ProductService.API.Entities;
using Tajan.ProductService.Infrastructure.Persistence.Extensions;
using Tajan.Standard.Infrastructure.Persistence.Contexts;

namespace Tajan.ProductService.Infrastructure.DbContexts;

public class CoreDbContext : SharedDbContext
{
    public CoreDbContext(DbContextOptions<CoreDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }

}