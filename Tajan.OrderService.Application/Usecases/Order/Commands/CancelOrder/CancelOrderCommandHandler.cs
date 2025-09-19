using MediatR;
using OrderAggregate = Tajan.OrderService.Domain.Entities.OrderAggregates;
using Tajan.Standard.Domain.Wrappers;
using Tajan.Standard.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Tajan.OrderService.Application.Usecases;

public class CancelOrderCommandHandler(IApplicationDbContext _context) : IRequestHandler<CancelOrderCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        OrderAggregate.Order? order =  await _context.Set<OrderAggregate.Order>()
            .FirstOrDefaultAsync(x => x.Id == request.OrderId, cancellationToken)
            ;

        if (order is null)
            return Result.Failure<bool>(error: Error.NotFoundItem, status: System.Net.HttpStatusCode.NotFound);
        
        order.CancelOrder();
        return Result.Success<bool>(data: true);
    }
}
