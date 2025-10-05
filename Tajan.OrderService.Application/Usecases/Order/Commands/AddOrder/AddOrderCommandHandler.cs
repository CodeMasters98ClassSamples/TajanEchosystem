using MediatR;
using OrderAggregate =  Tajan.OrderService.Domain.Entities.OrderAggregates;
using Tajan.Standard.Domain.Wrappers;
using Tajan.Standard.Domain.Abstratcions;
using Tajan.OrderService.Application.Contracts.ExternalServices;
using Microsoft.AspNetCore.Http;
using Tajan.Standard.Application.Abstractions;
using MassTransit;
using Tajan.OrderService.Domain.Entities.OrderAggregates;
using Grpc.Core;
using Grpc.Net.Client;
using Products.Grpc;

namespace Tajan.OrderService.Application.Usecases;

internal class AddOrderCommandHandler : IRequestHandler<AddOrderCommand, Result<int>>
{
    private readonly IProductService _ProductService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IApplicationDbContext _dbContext;
    public AddOrderCommandHandler(
        //IProductService ProductService,
        IHttpContextAccessor httpContextAccessor,
        IApplicationDbContext dbContext,
        IPublishEndpoint publishEndpoint)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
        //_ProductService = ProductService;
    }
    public async Task<Result<int>> Handle(AddOrderCommand request, CancellationToken cancellationToken)
    {
		try
		{
            List<OrderDetail> details = new List<OrderDetail> { };

            //Get User From Context
            var httpContext = _httpContextAccessor.HttpContext;
            var userId = httpContext?.Request.Headers["X-User-Id"].FirstOrDefault();

            var handler = new HttpClientHandler();
            // For HTTP (no TLS) in dev only:
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            for (int i = 0; i < request.Produts.Count; i++)
            {
                using var channel = GrpcChannel.ForAddress("https://localhost:7281", new GrpcChannelOptions
                {
                    HttpHandler = handler
                });
                var client = new Products.Grpc.Products.ProductsClient(channel);
                // Optional auth/JWT
                //var headers = new Metadata { { "Authorization", "Bearer " + jwtToken } };
                var headers = new Metadata { };
                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3)); // deadline/timeout
                var reply = await client.GetProductAsync(new GetProductRequest { Id = request.Produts[i].Id }, headers, cancellationToken: cts.Token);
                OrderDetail detail = OrderDetail.Create(productId: request.Produts[i].Id, price: reply.Amount);
                details.Add(detail);
            }

            var order = OrderAggregate.Order.Create(description: request.Description, userId: userId, details: details);
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
