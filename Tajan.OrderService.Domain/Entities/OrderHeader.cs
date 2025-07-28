using Tajan.OrderService.Domain.Enums;

namespace Tajan.OrderService.Domain.Entities;

public class OrderHeader
{
    public int Id { get; set; }
    public string Description { get; set; }
    public int UserId { get; set; }
    public Status Status { get; set; }
    public DateTime CreateAt { get; set; }
}
