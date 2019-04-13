using System.Collections.Generic;
using System.Globalization;

namespace Discounts
{
    public class ShipmentCost<TShipment> where TShipment: IShipment
    {
        public TShipment Shipment { get; }
        public decimal Price { get; }
        public decimal Discount { get; }

        public ShipmentCost(TShipment shipment, decimal price, decimal discount)
        {
            Shipment = shipment;
            Price = price;
            Discount = discount;
        }

        public ShipmentCost(TShipment shipment, Dictionary<(string, string), decimal> costReference)
            : this(shipment, shipment.GetCost(costReference), 0.0m)
        { }

        public string Format(string separator, List<string> dateFormats)
        {
            var formattedShipment = Shipment.Format();

            var format = string.Join(
                separator,
                formattedShipment,
                Price.ToString("F", CultureInfo.InvariantCulture),
                Discount == 0 ? "-" : Discount.ToString("F", CultureInfo.InvariantCulture));
            return format;
        }

    }
}