using System.Net.Http.Json;
using Tajan.OrderService.Application.Contracts.ExternalServices;
using Tajan.OrderService.Domain.ExtrenalModels;

namespace Tajan.OrderService.Infrastructure.ExternalServices;

public class ProductService: IProductService
{
    private readonly HttpClient _client;

    public ProductService(HttpClient client)
    {
        _client = client;
    }

    public async Task<List<Product>?> GetProductsAsync()
    {
        List<Product>? user = await _client.GetFromJsonAsync<List<Product>>($"objects");

        return user;
    }

    
}

