using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tajan.OrderService.Domain.Entities;

public class OrderDetail
{
    public int OrderHeaderId { get; set; }
    public int ProductId { get; set; }
}
