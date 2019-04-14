using System.Collections.Generic;
using System.Linq;
using Discounts;
using Discounts.Discounters;

namespace ConsoleApp.Tests
{
    internal class CompleteDiscounts : Discounter
    {
        public IEnumerable<IShipmentCost<IShipment>> Discount(IEnumerable<IShipmentCost<IShipment>> pricedShipments)
        {
            return pricedShipments.Select(RenderShipmentFree);
        }

        private static IShipmentCost<IShipment> RenderShipmentFree(IShipmentCost<IShipment> x)
        {
            if (x is GoodShipmentCost g)
            {
                var freeShipment = new GoodShipmentCost(g.Shipment, 0, 0);
                return freeShipment;
            }

            return x;

        }
    }
}