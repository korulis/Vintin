using System.Collections.Generic;

namespace Discounts
{
    public class NoDiscounts : IDiscounter
    {
        public IEnumerable<ProcessedShippingEntry> Discount(IEnumerable<ProcessedShippingEntry> shippingEntriesWithCosts)
        {
            return shippingEntriesWithCosts;
        }
    }
}