using MediatR;
using Tajan.Standard.Domain.Wrappers;

namespace Tajan.OrderService.Application.Usecases.Order;

public record AddOrderWithBasketCommand(int BasketId,string Description) : IRequest<Result<int>>;

