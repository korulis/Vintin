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

        public IEnumerable<ShipmentCost> CalculatePrice(IEnumerable<Shipment> shipments)
        {
            var shipmentPrices = shipments.Select(CalculatePrice);
            var pricedShipments = _discounter.Discount(shipmentPrices);
            return pricedShipments;
        }

        public ShipmentCost CalculatePrice(Shipment shipment)
        {
            if (shipment.IsCorrupt)
            {
                return new ShipmentCost(shipment, 0, 0);
            }

            var cost = _costReference[(shipment.PackageSize, shipment.ShippingProvider)];


            var shipmentWithPrice = new ShipmentCost(shipment, cost, 0.00m);
            return shipmentWithPrice;
        }

    }
}