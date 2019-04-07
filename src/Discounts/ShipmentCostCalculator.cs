using System.Collections.Generic;
using System.Linq;
using Discounts.Discounters;

namespace Discounts
{
    public class ShipmentCostCalculator
    {
        private readonly IDiscounter _discounter;

        private readonly Dictionary<(string, string), decimal> _costReference;

        public ShipmentCostCalculator(IDiscounter discounter, Dictionary<(string, string), decimal> sizeAndProviderToCost)
        {
            _discounter = discounter;
            _costReference = sizeAndProviderToCost;
        }

        public IEnumerable<ShipmentCost> CalculatePrice(IEnumerable<Shipment> shipments)
        {
            var shipmentCosts = shipments.Select(CalculateCost);
            var pricedShipments = _discounter.Discount(shipmentCosts);
            return pricedShipments;
        }

        public ShipmentCost CalculateCost(Shipment shipment)
        {
            if (shipment.IsCorrupt)
            {
                return new ShipmentCost(shipment, 0, 0);
            }

            var shipmentCost = _costReference[(shipment.PackageSize, shipment.ShippingProvider)];


            var result = new ShipmentCost(shipment, shipmentCost, 0.00m);
            return result;
        }

    }
}