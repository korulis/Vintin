using System.Collections.Generic;

namespace Discounts.Discounters
{
    public interface Discounter
    {
        IEnumerable<ShipmentCost<IShipment>> Discount(IEnumerable<ShipmentCost<IShipment>> pricedShipments);
    }
}