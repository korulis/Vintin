using System.Collections.Generic;
using System.Linq;
using Discounts.Discounters;

namespace Discounts
{
    public class ShippingCostCalculator
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

        public ShippingCostCalculator(IDiscounter discounter)
        {
            _discounter = discounter;
        }

        public IEnumerable<ShippingCostEntry> CalculatePrice(IEnumerable<ShippingEntry> shippingEntries)
        {
            var shippingEntriesWithPrices = shippingEntries.Select(CalculateInitialPrice);
            var pricedShippingEntries = _discounter.Discount(shippingEntriesWithPrices);
            return pricedShippingEntries;
        }

        public ShippingCostEntry CalculateInitialPrice(ShippingEntry shippingEntry)
        {
            if (shippingEntry.IsCorrupt)
            {
                return new ShippingCostEntry(shippingEntry, 0, 0);
            }

            var shippingCost = _costReference[(shippingEntry.PackageSize, shippingEntry.ShippingProvider)];


            var result = new ShippingCostEntry(shippingEntry, shippingCost, 0.00m);
            return result;
        }

    }
}