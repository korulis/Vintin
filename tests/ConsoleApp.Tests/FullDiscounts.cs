using System.Collections.Generic;
using System.Linq;
using Discounts;

namespace ConsoleApp.Tests
{
    internal class FullDiscounts : IDiscounter
    {
        public IEnumerable<ProcessedShippingEntry> Discount(IEnumerable<ProcessedShippingEntry> shippingEntriesWithCosts)
        {
            return shippingEntriesWithCosts.Select(ProcessedShippingEntry);
        }

        private static ProcessedShippingEntry ProcessedShippingEntry(ProcessedShippingEntry x)
        {
            var processedShippingEntry = new ProcessedShippingEntry(x.ShippingEntry, 0, 0);
            return processedShippingEntry;
        }
    }
}