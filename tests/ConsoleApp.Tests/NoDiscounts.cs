using System.Collections.Generic;
using Discounts;

namespace ConsoleApp.Tests
{
    public class NoDiscounts : IDiscounter
    {
        public IEnumerable<ShippingCostEntry> Discount(IEnumerable<ShippingCostEntry> pricedShippingEntries)
        {
            return pricedShippingEntries;
        }
    }
}