using MediatR;
using Tajan.Standard.Domain.Wrappers;

namespace Tajan.OrderService.Application.Usecases.Order.Commands.CancelOrder;

public record CancelOrderCommand(int OrderId) : IRequest<Result<bool>>;
