using System.Collections.Generic;

namespace Discounts
{
    public interface IDiscounter
    {
        IEnumerable<ProcessedShippingEntry> Discount(IEnumerable<ProcessedShippingEntry> shippingEntriesWithCosts);
    }
}