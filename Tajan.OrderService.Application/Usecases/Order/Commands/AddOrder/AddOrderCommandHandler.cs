using MediatR;
using OrderAggregate =  Tajan.OrderService.Domain.Entities.OrderAggregates;
using Tajan.Standard.Domain.Wrappers;
using Tajan.Standard.Domain.Abstratcions;
using Tajan.OrderService.Application.Contracts.ExternalServices;

namespace Tajan.OrderService.Application.Usecases;

internal class AddOrderCommandHandler : IRequestHandler<AddOrderCommand, Result<int>>
{
    private readonly IProductService _ProductService;
    public AddOrderCommandHandler(IProductService ProductService)
    {
        _ProductService = ProductService;
    }
    public async Task<Result<int>> Handle(AddOrderCommand request, CancellationToken cancellationToken)
    {
		try
		{
            //Get Products Data?
            //var products = _ProductService.GetProductsAsync();
            //if (products is null)
            //{
            //    //Appliction exception
            //}

            var order = OrderAggregate.Order.Create(description: request.Description, userId: 1, details: null);
            return Result.Success<int>(data: order.Id);

            //200 -> isSuccess =true | false

            //300 -> Problem Detail
            //400
            //500
        }
		catch (DomainException ex)
		{
			throw;
		}
    }
}
