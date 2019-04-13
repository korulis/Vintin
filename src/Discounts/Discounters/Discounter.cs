using System.Collections.Generic;

namespace Discounts.Discounters
{
    public interface Discounter
    {
        IEnumerable<IShipmentCost<IShipment>> Discount(IEnumerable<IShipmentCost<IShipment>> pricedShipments);
    }
}