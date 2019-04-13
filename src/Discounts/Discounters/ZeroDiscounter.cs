using System.Collections.Generic;

namespace Discounts.Discounters
{
    public class ZeroDiscounter : Discounter
    {
        public IEnumerable<IShipmentCost<IShipment>> Discount(IEnumerable<IShipmentCost<IShipment>> pricedShipments)
        {
            return pricedShipments;
        }
    }
}