using MediatR;

namespace Tajan.OrderService.Application.Usecases.Order.Commands.RemoveBasketItem;

public record RemoveBasketItemCommand(int UserId, int ProductId) : IRequest<Unit>;
