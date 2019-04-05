using System.Collections.Generic;
using System.Linq;
using Discounts.Discounters;

namespace Discounts
{
    public class ShippingCostCalculator
    {
        private readonly IDiscounter _discounter;

        private readonly Dictionary<(string, string), decimal> _costReference;

        public ShippingCostCalculator(IDiscounter discounter, Dictionary<(string, string), decimal> sizeAndProviderToCost)
        {
            _discounter = discounter;
            _costReference = sizeAndProviderToCost;
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