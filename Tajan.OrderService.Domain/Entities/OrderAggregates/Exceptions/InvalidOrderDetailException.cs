using Tajan.Standard.Domain.Abstratcions;

namespace Tajan.OrderService.Domain.Entities.OrderAggregates.Exceptions;

public class InvalidOrderDetailException : DomainException
{
    public InvalidOrderDetailException()
     : base($"Order Detail Is Invalid.") { }

    public override string Code => "order.inavlid_detail";
}
