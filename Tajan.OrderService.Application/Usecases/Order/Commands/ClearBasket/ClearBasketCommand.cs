using MediatR;

namespace Tajan.OrderService.Application.Usecases.Order.Commands.ClearBasket;

public record ClearBasketCommand(int UserId) : IRequest<Unit>;
