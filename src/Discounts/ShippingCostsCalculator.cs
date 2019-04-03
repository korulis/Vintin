using System.Collections.Generic;
using System.Linq;

namespace Discounts
{
    public class ShippingCostsCalculator
    {
        public IEnumerable<ProcessedShippingEntry> CalculateCost(IEnumerable<ShippingEntry> shippingEntries)
        {
            return shippingEntries
                .Select(x => new ProcessedShippingEntry(x, 0.00m, 0.00m));
        }

    }
}