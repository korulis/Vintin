using System.Collections.Generic;

namespace Discounts.Discounters
{
    public class ZeroDiscounter : IDiscounter
    {
        public IEnumerable<ShipmentCost> Discount(IEnumerable<ShipmentCost> pricedShipment)
        {
            return pricedShipment;
        }
    }
}