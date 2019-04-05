using System.Collections.Generic;
using System.Linq;

namespace Discounts
{
    public class ShippingPriceCalculator
    {
        private readonly IDiscounter _discounter;

        private readonly Dictionary<(string, string), decimal> _costReference =
            new Dictionary<(string, string), decimal>
            {
                { ("S","LP"), 1.50m},
                { ("M","LP"), 4.90m},
                { ("L","LP"), 6.90m},
                { ("S","MR"), 2.00m},
                { ("M","MR"), 3.00m},
                { ("L","MR"), 4.00m}
            };

        public ShippingPriceCalculator(IDiscounter discounter)
        {
            _discounter = discounter;
        }

        public IEnumerable<ProcessedShippingEntry> CalculatePrice(IEnumerable<ShippingEntry> shippingEntries)
        {
            var shippingEntriesWithCosts = shippingEntries.Select(CalculateCost);
            var processedShippingEntries = _discounter.Discount(shippingEntriesWithCosts);
            return processedShippingEntries;
        }

        public ProcessedShippingEntry CalculateCost(ShippingEntry shippingEntry)
        {
            if (shippingEntry.IsCorrupt)
            {
                return new ProcessedShippingEntry(shippingEntry, 0, 0);
            }

            var shippingCost = _costReference[(shippingEntry.PackageSize, shippingEntry.ShippingProvider)];


            var result = new ProcessedShippingEntry(shippingEntry, shippingCost, 0.00m);
            return result;
        }

    }
}