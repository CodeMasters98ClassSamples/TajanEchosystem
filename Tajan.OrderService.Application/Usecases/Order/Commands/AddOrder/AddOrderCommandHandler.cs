using MediatR;
using OrderAggregate =  Tajan.OrderService.Domain.Entities.OrderAggregates;
using Tajan.Standard.Domain.Wrappers;
using Tajan.Standard.Domain.Abstratcions;
using Tajan.OrderService.Application.Contracts.ExternalServices;
using Microsoft.AspNetCore.Http;
using Tajan.Standard.Application.Abstractions;
using Tajan.OrderService.Domain.Entities.OrderAggregates;
using MassTransit;
using MassTransit.Transports;
using Tajan.Standard.Application.ServiceIngtegrations.NotificationService;

namespace Tajan.OrderService.Application.Usecases;

internal class AddOrderCommandHandler : IRequestHandler<AddOrderCommand, Result<int>>
{
    private readonly IProductService _ProductService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IApplicationDbContext _dbContext;
    private readonly IPublishEndpoint _publishEndpoint;
    public AddOrderCommandHandler(
        //IProductService ProductService,
        IHttpContextAccessor httpContextAccessor,
        IApplicationDbContext dbContext,
        IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
        //_ProductService = ProductService;
    }
    public async Task<Result<int>> Handle(AddOrderCommand request, CancellationToken cancellationToken)
    {
		try
		{
            await _publishEndpoint.Publish<SendSingleSms>(new
            {
                MobileNumber = "09129564205",
                Content = "09129564205",
                Title = "09129564205",
            });
            var httpContext = _httpContextAccessor.HttpContext;
            var userId = httpContext?.Request.Headers["X-User-Id"].FirstOrDefault();

            //Get Products Data?
            //var products = _ProductService.GetProductsAsync();
            //if (products is null)
            //{
            //    //Appliction exception
            //}
            var order = OrderAggregate.Order.Create(description: request.Description, userId: 1, details: null);
            _dbContext.Set<OrderAggregate.Order>().Add(order);
            await _dbContext.SaveChangesAsync();
            return Result.Success<int>(data: order.Id);

        }
		catch (DomainException ex)
		{
			throw;
		}
    }
}
