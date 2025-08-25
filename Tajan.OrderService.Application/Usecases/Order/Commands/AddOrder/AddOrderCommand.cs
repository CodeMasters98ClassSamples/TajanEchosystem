using MediatR;
using Tajan.OrderService.Domain.Entities.OrderAggregates;
using Tajan.Standard.Domain.Wrappers;

namespace Tajan.OrderService.Application.Usecases;

public class AddOrderCommand : IRequest<Result<Order>>
{
    public string Description { get; set; }
}
