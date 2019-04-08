using System.Collections.Generic;

namespace Discounts.Discounters
{
    public class ZeroDiscounter : Discounter
    {
        public IEnumerable<ShipmentCost> Discount(IEnumerable<ShipmentCost> pricedShipments)
        {
            return pricedShipments;
        }
    }
}