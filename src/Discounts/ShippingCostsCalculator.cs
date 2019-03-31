using System.Collections.Generic;
using System.Linq;

namespace Discounts
{
    public class ShippingCostsCalculator
    {
        public IEnumerable<DiscountedShippingEntry> CalculateCost(IEnumerable<ShippingEntry> shippingEntries)
        {
            return shippingEntries
                .Select(x => new DiscountedShippingEntry(x, "0.00", "-"));
        }

    }
}