using System.Collections.Generic;
using System.Linq;
using Discounts;
using Discounts.Discounters;

namespace ConsoleApp.Tests
{
    internal class CompleteDiscounts : IDiscounter
    {
        public IEnumerable<ShippingCostEntry> Discount(IEnumerable<ShippingCostEntry> pricedShippingEntries)
        {
            return pricedShippingEntries.Select(ProcessedShippingEntry);
        }

        private static ShippingCostEntry ProcessedShippingEntry(ShippingCostEntry x)
        {
            var processedShippingEntry = new ShippingCostEntry(x.ShippingEntry, 0, 0);
            return processedShippingEntry;
        }
    }
}