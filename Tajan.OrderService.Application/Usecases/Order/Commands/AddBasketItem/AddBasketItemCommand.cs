using MediatR;

namespace Tajan.OrderService.Application.Usecases.Order.Commands.AddBasketItem;

public record AddBasketItemCommand(int UserId, int ProductId, int Quantity) : IRequest<Unit>;
