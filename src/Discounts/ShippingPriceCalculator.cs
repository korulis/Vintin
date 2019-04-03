using System.Collections.Generic;
using System.Linq;

namespace Discounts
{
    public class ShippingPriceCalculator
    {
        public IEnumerable<ProcessedShippingEntry> CalculatePrice(IEnumerable<ShippingEntry> shippingEntries)
        {
            return shippingEntries
                .Select(CalculateCost);
        }

        public ProcessedShippingEntry CalculateCost(ShippingEntry shippingEntry)
        {
            return new ProcessedShippingEntry(shippingEntry, 0.00m, 0.00m);
        }

    }
}