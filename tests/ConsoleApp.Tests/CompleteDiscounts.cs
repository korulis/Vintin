﻿using System.Collections.Generic;
using System.Linq;
using Discounts;
using Discounts.Discounters;

namespace ConsoleApp.Tests
{
    internal class CompleteDiscounts : Discounter
    {
        public IEnumerable<ShipmentCost> Discount(IEnumerable<ShipmentCost> pricedShipments)
        {
            return pricedShipments.Select(RenderShipmentFree);
        }

        private static ShipmentCost RenderShipmentFree(ShipmentCost x)
        {
            var freeShipment = new ShipmentCost(x.Shipment, 0, 0);
            return freeShipment;
        }
    }
}