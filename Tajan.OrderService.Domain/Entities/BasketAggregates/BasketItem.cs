using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tajan.OrderService.Domain.Abstractions;

namespace Tajan.OrderService.Domain.Entities.BasketAggregates
{
    internal class BasketItem: Entity
    {
        public int MyProperty { get; set; }
    }
}
