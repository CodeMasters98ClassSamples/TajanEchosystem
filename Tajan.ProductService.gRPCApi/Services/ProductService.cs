using Grpc.Core;
using Products.Grpc;

namespace Tajan.ProductService.gRPCApi.Services;

public class ProductService : Products.Grpc.Products.ProductsBase
{
    public override Task<GetProductResponse> GetProduct(GetProductRequest request, ServerCallContext context)
    {
        var result = new GetProductResponse
        {
            Amount = 2000,
            Name = "PAID"
        };
        return Task.FromResult(result);
    }
}
