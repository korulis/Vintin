using System.Collections.Generic;

namespace Discounts
{
    public interface IDiscounter
    {
        IEnumerable<ShippingCostEntry> Discount(IEnumerable<ShippingCostEntry> pricedShippingEntries);
    }
}