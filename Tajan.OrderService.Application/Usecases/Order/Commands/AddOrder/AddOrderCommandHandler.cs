using MediatR;
using Tajan.OrderService.Domain.Entities;
using Tajan.Standard.Domain.Wrappers;

namespace Tajan.OrderService.Application.Usecases;

internal class AddOrderCommandHandler : IRequestHandler<AddOrderCommand, Result<OrderHeader>>
{
    public async Task<Result<OrderHeader>> Handle(AddOrderCommand request, CancellationToken cancellationToken)
    {
        var order = new OrderHeader() { Id = 1, Description = "" };
        return Result.Success<OrderHeader>(data: order);
    }
}
