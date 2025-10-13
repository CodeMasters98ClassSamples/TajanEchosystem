using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Products.Grpc;
using Tajan.ProductService.API.Entities;
using Tajan.ProductService.Infrastructure.DbContexts;
using Tajan.Standard.Application.Abstractions;
using Tajan.Standard.Domain.Abstratcions;

namespace Tajan.ProductService.gRPCApi.Services;

public class ProductService : Products.Grpc.Products.ProductsBase
{
    private readonly CoreDbContext _dbContext;
    private readonly ICacheProvider _cacheProvider;
    public ProductService(CoreDbContext dbContext, ICacheProvider cacheProvider)
    {
        _cacheProvider = cacheProvider;
        _dbContext = dbContext; 
    }

    public override async Task<GetProductResponse> GetProduct(GetProductRequest request, ServerCallContext context)
    {
        Product product = await _cacheProvider
                    .GetAsync<Product>(CacheKey.Product(request.Id), default);
        if (product is null)
        {
            product = await _dbContext.Set<Product>().FirstOrDefaultAsync(x => x.Id == request.Id);
            await _cacheProvider.SetAsync(CacheKey.Product(request.Id), product, default);
        }


        var result = new GetProductResponse
        {
            Amount = product.Price,
            Name = product.Name
        };
        return await Task.FromResult(result);
    }
}
