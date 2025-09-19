using MediatR;
using Tajan.Standard.Domain.Wrappers;

namespace Tajan.OrderService.Application.Usecases;

public class AddOrderCommand : IRequest<Result<int>>
{
    public string Description { get; set; }
    public List<ProductDto> Produts { get; set; }
}


public class ProductDto
{
    public int Id { get; set; }
}