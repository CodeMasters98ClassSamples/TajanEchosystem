using MediatR;
using Tajan.Standard.Domain.Wrappers;

namespace Tajan.OrderService.Application.Usecases;

public record CancelOrderCommand(int OrderId) : IRequest<Result<bool>>;
