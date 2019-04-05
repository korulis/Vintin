using System.Collections.Generic;

namespace Discounts.Discounters
{
    public interface IDiscounter
    {
        IEnumerable<ShippingCostEntry> Discount(IEnumerable<ShippingCostEntry> pricedShippingEntries);
    }
}