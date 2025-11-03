using MediatR;
using Tajan.OrderService.Domain.Entities.BasketAggregates.Repositories;

namespace Tajan.OrderService.Application.Usecases.Order.Commands.ClearBasket;

public class ClearBasketCommandHandler : IRequestHandler<ClearBasketCommand, Unit>
{
    private readonly IBasketRepository _basketRepository;

    public ClearBasketCommandHandler(IBasketRepository basketRepository)
    {
        _basketRepository = basketRepository;
    }

    public async Task<Unit> Handle(ClearBasketCommand request, CancellationToken cancellationToken)
    {
        await _basketRepository.ClearBasketAsync(request.UserId);
        return Unit.Value;
    }
}
