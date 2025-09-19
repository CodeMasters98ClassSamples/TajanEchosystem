using MediatR;
using Tajan.OrderService.Domain.Services;
using Tajan.Standard.Domain.Wrappers;

namespace Tajan.OrderService.Application.Usecases.Order;

public class AddOrderWithBasketCommandHandler(IOrderService orderService) : IRequestHandler<AddOrderWithBasketCommand, Result<int>>
{
    public async Task<Result<int>> Handle(AddOrderWithBasketCommand request, CancellationToken cancellationToken)
    {
        SaveOrderDto saveOrderDto = null;
        await orderService.SaveOrder(saveOrderDto);

        return Result.Success<int>(data: 1);
    }
}
