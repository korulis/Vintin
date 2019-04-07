﻿using System.Collections.Generic;

namespace Discounts.Discounters
{
    public interface IDiscounter
    {
        IEnumerable<ShipmentCost> Discount(IEnumerable<ShipmentCost> pricedShipment);
    }
}