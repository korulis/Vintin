using System.Collections.Generic;

namespace Discounts.Discounters
{
    public class ZeroDiscounter : IDiscounter
    {
        public IEnumerable<ShippingCostEntry> Discount(IEnumerable<ShippingCostEntry> pricedShippingEntries)
        {
            return pricedShippingEntries;
        }
    }
}