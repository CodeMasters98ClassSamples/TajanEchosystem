using Tajan.ProductService.API.Contracts;
using Tajan.ProductService.API.Entities;
using Tajan.ProductService.Infrastructure.DbContexts;

namespace Tajan.ProductService.API.Services;

public class ProductBusiness : IProductService
{
    private readonly CoreDbContext _dbContext;

    public ProductBusiness(CoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public int Add(Product item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        _dbContext.Products.Add(item);
        _dbContext.SaveChanges();

        return item.Id;
    }
}
