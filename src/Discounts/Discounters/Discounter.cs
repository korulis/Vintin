using System.Collections.Generic;

namespace Discounts.Discounters
{
    public interface Discounter
    {
        IEnumerable<ShipmentCost> Discount(IEnumerable<ShipmentCost> pricedShipments);
    }
}