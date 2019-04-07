using System.Collections.Generic;
using System.Linq;
using Discounts;
using Discounts.Discounters;

namespace ConsoleApp.Tests
{
    internal class CompleteDiscounts : IDiscounter
    {
        public IEnumerable<ShipmentCost> Discount(IEnumerable<ShipmentCost> pricedShipment)
        {
            return pricedShipment.Select(RenderShipmentFree);
        }

        private static ShipmentCost RenderShipmentFree(ShipmentCost x)
        {
            var freeShipment = new ShipmentCost(x.Shipment, 0, x.Price + x.Discount);
            return freeShipment;
        }
    }
}