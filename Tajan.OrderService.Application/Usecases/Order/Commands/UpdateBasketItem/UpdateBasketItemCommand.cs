using MediatR;

namespace Tajan.OrderService.Application.Usecases.Order.Commands.UpdateBasketItem;

public record UpdateBasketItemCommand(int UserId, int ProductId, int Quantity) : IRequest<Unit>;
