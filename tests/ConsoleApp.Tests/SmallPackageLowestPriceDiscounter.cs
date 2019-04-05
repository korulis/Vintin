using System.Collections.Generic;
using Discounts;

namespace ConsoleApp.Tests
{
    public class SmallPackageLowestPriceDiscounter : IDiscounter
    {
        public IEnumerable<ProcessedShippingEntry> Discount(IEnumerable<ProcessedShippingEntry> shippingEntriesWithCosts)
        {
            throw new System.NotImplementedException();
        }
    }
}