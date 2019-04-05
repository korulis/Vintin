using System.Collections.Generic;
using Discounts;

namespace ConsoleApp.Tests
{
    public class NoDiscounts : IDiscounter
    {
        public IEnumerable<ProcessedShippingEntry> Discount(IEnumerable<ProcessedShippingEntry> shippingEntriesWithCosts)
        {
            return shippingEntriesWithCosts;
        }
    }
}