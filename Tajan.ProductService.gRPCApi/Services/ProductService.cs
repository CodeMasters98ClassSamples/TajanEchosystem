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
        try
        {
            // defensive null checks for injected dependencies
            if (_dbContext == null)
                throw new RpcException(new Status(StatusCode.Internal, "Database context is not available"));

            Product? product = null;

            if (_cacheProvider != null)
            {
                try
                {
                    product = await _cacheProvider.GetAsync<Product>(CacheKey.Product(request.Id), default);
                }
                catch
                {
                    // ignore cache errors and continue to DB
                    product = null;
                }
            }

            if (product is null)
            {
                product = await _dbContext.Set<Product>().FirstOrDefaultAsync(x => x.Id == request.Id);

                if (_cacheProvider != null)
                {
                    try
                    {
                        await _cacheProvider.SetAsync(CacheKey.Product(request.Id), product, default);
                    }
                    catch
                    {
                        // ignore cache set errors
                    }
                }
            }

            if (product is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Product {request.Id} not found"));
            }

            var result = new GetProductResponse
            {
                Amount = product.Price,
                Name = product.Name
            };

            return result;
        }
        catch (RpcException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new RpcException(new Status(StatusCode.Internal, $"Internal server error: {ex.Message}"));
        }
    }
}
