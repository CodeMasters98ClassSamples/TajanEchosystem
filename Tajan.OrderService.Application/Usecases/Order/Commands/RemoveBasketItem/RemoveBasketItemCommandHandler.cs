using MediatR;
using Tajan.OrderService.Domain.Entities.BasketAggregates.Repositories;

namespace Tajan.OrderService.Application.Usecases.Order.Commands.RemoveBasketItem;

public class RemoveBasketItemCommandHandler : IRequestHandler<RemoveBasketItemCommand, Unit>
{
    private readonly IBasketRepository _basketRepository;

    public RemoveBasketItemCommandHandler(IBasketRepository basketRepository)
    {
        _basketRepository = basketRepository;
    }

    public async Task<Unit> Handle(RemoveBasketItemCommand request, CancellationToken cancellationToken)
    {
        await _basketRepository.RemoveItemAsync(request.UserId, request.ProductId);
        return Unit.Value;
    }
}
