using MediatR;
using Tajan.OrderService.Domain.Entities;
using Tajan.Standard.Domain.Wrappers;

namespace Tajan.OrderService.Application.Usecases;

public class AddOrderCommand : IRequest<Result<OrderHeader>>
{
    public string Description { get; set; }
}
