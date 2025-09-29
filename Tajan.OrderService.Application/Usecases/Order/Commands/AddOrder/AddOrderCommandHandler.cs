using MediatR;
using OrderAggregate =  Tajan.OrderService.Domain.Entities.OrderAggregates;
using Tajan.Standard.Domain.Wrappers;
using Tajan.Standard.Domain.Abstratcions;
using Tajan.OrderService.Application.Contracts.ExternalServices;
using Microsoft.AspNetCore.Http;

namespace Tajan.OrderService.Application.Usecases;

internal class AddOrderCommandHandler : IRequestHandler<AddOrderCommand, Result<int>>
{
    private readonly IProductService _ProductService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public AddOrderCommandHandler(
        //IProductService ProductService,
        IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        //_ProductService = ProductService;
    }
    public async Task<Result<int>> Handle(AddOrderCommand request, CancellationToken cancellationToken)
    {
		try
		{
            var httpContext = _httpContextAccessor.HttpContext;
            var userId = httpContext?.Request.Headers["X-User-Id"].FirstOrDefault();

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
