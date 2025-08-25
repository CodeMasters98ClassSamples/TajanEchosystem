using MediatR;
using OrderAggregate =  Tajan.OrderService.Domain.Entities.OrderAggregates;
using Tajan.Standard.Domain.Wrappers;
using Tajan.OrderService.Domain.Entities.OrderAggregates;

namespace Tajan.OrderService.Application.Usecases;

internal class AddOrderCommandHandler : IRequestHandler<AddOrderCommand, Result<OrderAggregate.Order>>
{
    public async Task<Result<OrderAggregate.Order>> Handle(AddOrderCommand request, CancellationToken cancellationToken)
    {
        var order = OrderAggregate.Order.Create(description: request.Description, userId: 1, details: null); // Order is Aggergate Root
        

        return Result.Success<OrderAggregate.Order>(data: order);
    }
}
