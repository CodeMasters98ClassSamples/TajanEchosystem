using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tajan.OrderService.Domain.Entities.OrderAggregates.ValueObjects
{
    public sealed class Price
    {
        public Price(decimal amount, string currency)
        {
            if (amount <= 0)
                throw new Exception();

            if (string.IsNullOrWhiteSpace(currency))
                throw new Exception();

            Amount = amount;
            Currency = currency;
        }
        public decimal Amount { get; private set; }
        public string Currency { get; private set; }

    }
}
