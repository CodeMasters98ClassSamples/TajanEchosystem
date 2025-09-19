using Tajan.Standard.Domain.Abstratcions;

namespace Tajan.OrderService.Domain.Entities.OrderAggregates.Exceptions;

public class InvalidUserException : DomainException
{
    public InvalidUserException() : base($"Invalid User!") { }

    public override string Code => "order.inavlid_user";
}
