using Tajan.ProductService.API.Entities;

namespace Tajan.ProductService.API.Contracts
{
    public interface IProductService
    {
        int Add(Product item);
    }
}
