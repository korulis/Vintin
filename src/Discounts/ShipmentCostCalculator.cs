using System;
using System.Collections.Generic;
using System.Linq;
using Discounts.Discounters;

namespace Discounts
{
    public class ShipmentCostCalculator
    {
        private readonly Discounter _discounter;

        private readonly Dictionary<(string, string), decimal> _costReference;

        public ShipmentCostCalculator(Discounter discounter, Dictionary<(string, string), decimal> sizeAndProviderToCost)
        {
            _discounter = discounter;
            _costReference = sizeAndProviderToCost;
        }

        public IEnumerable<IShipmentCost<IShipment>> CalculatePrice(IEnumerable<IShipment> shipments)
        {
            var shipmentPrices = shipments.Select(CalculatePrice);
            var pricedShipments = _discounter.Discount(shipmentPrices);
            return pricedShipments;
        }

        public IShipmentCost<IShipment> CalculatePrice(IShipment shipment)
        {
            if (shipment is IgnoredShipment c)
            {
                return new IgnoredShipmentCost(c);
            }

            if (shipment is Shipment g)
            {
                return new GoodShipmentCost(g, _costReference);
            }

            throw new NotImplementedException();
        }

    }
}