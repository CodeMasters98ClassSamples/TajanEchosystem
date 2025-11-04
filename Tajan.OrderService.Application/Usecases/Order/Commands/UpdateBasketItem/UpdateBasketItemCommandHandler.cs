using MediatR;
using Tajan.OrderService.Application.Contracts.ExternalServices;
using Tajan.OrderService.Domain.Entities.BasketAggregates.Repositories;

namespace Tajan.OrderService.Application.Usecases.Order.Commands.UpdateBasketItem;

public class UpdateBasketItemCommandHandler : IRequestHandler<UpdateBasketItemCommand, Unit>
{
    private readonly IBasketRepository _basketRepository;
    private readonly IProductService _productService;

    public UpdateBasketItemCommandHandler(IBasketRepository basketRepository, IProductService productService)
    {
        _basketRepository = basketRepository;
        _productService = productService;
    }

    public async Task<Unit> Handle(UpdateBasketItemCommand request, CancellationToken cancellationToken)
    {
        // Validate product exists to obtain price
        var products = await _productService.GetProductsAsync();
        var product = products?.FirstOrDefault(p => p.Id == request.ProductId);
        if (product == null)
            throw new KeyNotFoundException("Product not found");

        decimal unitPrice = product.Price;

        await _basketRepository.AddOrUpdateItemAsync(request.UserId, request.ProductId, request.Quantity, unitPrice);

        return Unit.Value;
    }
}
