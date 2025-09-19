using Tajan.OrderService.Domain.ExtrenalModels;

namespace Tajan.OrderService.Application.Contracts.ExternalServices;

public interface IProductService
{
    Task<List<Product>?> GetProductsAsync();
}
