using System.Collections.Generic;
using System.Globalization;

namespace Discounts
{
    public class GoodShipmentCost : IShipmentCost<Shipment>
    {
        public Shipment Shipment { get; }
        public decimal Price { get; }
        public decimal Discount { get; }

        public GoodShipmentCost(Shipment shipment, decimal price, decimal discount)
        {
            Shipment = shipment;
            Price = price;
            Discount = discount;
        }

        public GoodShipmentCost(Shipment shipment, Dictionary<(string, string), decimal> costReference)
            : this(shipment, shipment.GetCost(costReference), 0.0m) { }

        public string Format(string separator)
        {
            var formattedShipment = Shipment.Format();

            var format = string.Join(
                separator,
                formattedShipment,
                Price.ToString("F", CultureInfo.InvariantCulture),
                Discount == 0 ? "-" : Discount.ToString("F", CultureInfo.InvariantCulture));
            return format;
        }

        public IShipmentCost<Shipment> MinimizeCostForSize(decimal minCost, string targetSize)
        {
            if (Shipment.PackageSize == targetSize)
            {
                var oldDiscount = Discount;
                var oldPrice = Price;
                var newPrice = oldPrice > minCost ? minCost : oldPrice;
                var newDiscount = oldPrice - newPrice + oldDiscount;
                return new GoodShipmentCost(Shipment, newPrice, newDiscount);
            }
            return this;
        }
    }
}