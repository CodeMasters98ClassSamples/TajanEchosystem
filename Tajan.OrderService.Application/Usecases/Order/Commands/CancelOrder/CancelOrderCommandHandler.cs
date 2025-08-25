using MediatR;
using OrderAggregate = Tajan.OrderService.Domain.Entities.OrderAggregates;
using Tajan.Standard.Domain.Wrappers;

namespace Tajan.OrderService.Application.Usecases.Order.Commands.CancelOrder
{
    public class CancelOrderCommandHandler() : IRequestHandler<CancelOrderCommand, Result<bool>>
    {
        public Task<Result<bool>> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {

            OrderAggregate.Order order = null;
            order.Status = OrderAggregate.Enums.Status.CANCELED;
        }
    }
}
