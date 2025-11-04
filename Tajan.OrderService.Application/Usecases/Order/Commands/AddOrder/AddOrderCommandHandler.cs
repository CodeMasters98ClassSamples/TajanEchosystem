using MediatR;
using OrderAggregate =  Tajan.OrderService.Domain.Entities.OrderAggregates;
using Tajan.Standard.Domain.Wrappers;
using Tajan.Standard.Domain.Abstratcions;
using Tajan.OrderService.Application.Contracts.ExternalServices;
using Microsoft.AspNetCore.Http;
using Tajan.Standard.Application.Abstractions;
using Microsoft.Extensions.Configuration;
using MassTransit;
using Tajan.OrderService.Domain.Entities.OrderAggregates;
using Grpc.Core;
using Grpc.Net.Client;
using Products.Grpc;

namespace Tajan.OrderService.Application.Usecases;

internal class AddOrderCommandHandler : IRequestHandler<AddOrderCommand, Result<int>>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IApplicationDbContext _dbContext;
    private readonly IConfiguration _configuration;
    public AddOrderCommandHandler(
        //IProductService ProductService,
        IHttpContextAccessor httpContextAccessor,
        IApplicationDbContext dbContext,
        IPublishEndpoint publishEndpoint,
        IConfiguration configuration)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
        //_ProductService = ProductService;
        _configuration = configuration;
    }
    public async Task<Result<int>> Handle(AddOrderCommand request, CancellationToken cancellationToken)
    {
		try
		{
            List<OrderDetail> details = new List<OrderDetail> { };

            var httpContext = _httpContextAccessor.HttpContext;
            var userId = httpContext?.Request.Headers["X-User-Id"].FirstOrDefault();

            var handler = new HttpClientHandler();
            var grpcUrl = _configuration["PRODUCT_GRPC_URL"] ?? "https://localhost:7281";

            // If using unencrypted HTTP for gRPC in docker, enable HTTP/2 unencrypted support
            if (grpcUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
            {
                AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            }

            for (int i = 0; i < request.Produts.Count; i++)
            {
                try
                {
                    using var channel = GrpcChannel.ForAddress(grpcUrl, new GrpcChannelOptions
                    {
                        HttpHandler = handler
                    });
                    var client = new Products.Grpc.Products.ProductsClient(channel);
                    var headers = new Metadata { };
                    var reply = await client.GetProductAsync(new GetProductRequest { Id = request.Produts[i].Id }, headers);
                    OrderDetail detail = OrderDetail.Create(productId: request.Produts[i].Id, price: reply.Amount);
                    details.Add(detail);
                }
                catch (Grpc.Core.RpcException rpcEx) when (rpcEx.StatusCode == Grpc.Core.StatusCode.NotFound)
                {
                    return Result.Failure<int>(new Tajan.Standard.Domain.Wrappers.Error("Error.ProductNotFound", rpcEx.Status.Detail), System.Net.HttpStatusCode.NotFound, rpcEx.Status.Detail);
                }
            }

            var order = OrderAggregate.Order.Create(description: request.Description, userId: userId ?? string.Empty, details: details);
            _dbContext.Set<OrderAggregate.Order>().Add(order);
            await _dbContext.SaveChangesAsync();
            return Result.Success<int>(data: order.Id);

        }
        catch (DomainException)
        {
            throw;
        }
    }
}
