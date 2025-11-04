using MediatR;
using Tajan.OrderService.Application.Dtos;
using Tajan.OrderService.Domain.Entities.BasketAggregates.Repositories;

namespace Tajan.OrderService.Application.Usecases.Order.Queries.GetBasket;

public class GetBasketQueryHandler : IRequestHandler<GetBasketQuery, BasketDto?>
{
    private readonly IBasketRepository _basketRepository;

    public GetBasketQueryHandler(IBasketRepository basketRepository)
    {
        _basketRepository = basketRepository;
    }

    public async Task<BasketDto?> Handle(GetBasketQuery request, CancellationToken cancellationToken)
    {
        var basket = await _basketRepository.GetByUserIdAsync(request.UserId);
        if (basket == null) return null;

        var dto = new BasketDto
        {
            UserId = basket.UserId,
            Subtotal = 0m
        };

        if (basket.Items != null)
        {
            foreach (var item in basket.Items)
            {
                var unit = item.Price?.Amount ?? 0m;
                var line = unit * (item.Quantity == 0 ? 1 : item.Quantity);
                dto.Items.Add(new BasketItemDto
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = unit,
                    LineTotal = line
                });
                dto.Subtotal += line;
            }
        }

        return dto;
    }
}
