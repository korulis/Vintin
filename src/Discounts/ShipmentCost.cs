using System.Collections.Generic;
using System.Globalization;

namespace Discounts
{
    public class ShipmentCost
    {
        public IShipment Shipment { get; }
        public decimal Price { get; }
        public decimal Discount { get; }

        public ShipmentCost(IShipment shipment, decimal price, decimal discount)
        {
            Shipment = shipment;
            Price = price;
            Discount = discount;
        }

        public ShipmentCost(IShipment shipment, Dictionary<(string, string), decimal> costReference)
        {
            if (shipment is CorruptShipment)
            {
                return new ShipmentCost(shipment, 0, 0);
            }

            var cost = costReference[(shipment.PackageSize, shipment.ShippingProvider)];


            var shipmentWithPrice = new ShipmentCost(shipment, cost, 0.00m);
            return shipmentWithPrice;
        }

        public string Format(string separator, List<string> dateFormats)
        {
            var formattedShipment = Shipment.Format(separator, dateFormats);

            var format = string.Join(
                separator,
                formattedShipment,
                Price.ToString("F", CultureInfo.InvariantCulture),
                Discount == 0 ? "-" : Discount.ToString("F", CultureInfo.InvariantCulture));
            return format;
        }

    }
}