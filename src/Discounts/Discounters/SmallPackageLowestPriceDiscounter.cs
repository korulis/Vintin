using System.Collections.Generic;
using System.Linq;

namespace Discounts.Discounters
{
    public class SmallPackageLowestPriceDiscounter : Discounter
    {
        private readonly Discounter _underlying;
        private readonly decimal _minCost;
        private readonly string _discountedPackageSize;

        public SmallPackageLowestPriceDiscounter(Discounter underlying,Dictionary<(string, string), decimal> sizeAndProviderToCost)
        {
            _underlying = underlying;
            _discountedPackageSize = "S";
            _minCost = sizeAndProviderToCost.Where(x => x.Key.Item1 == _discountedPackageSize).Select(x => x.Value).Min();
        }

        public IEnumerable<ShipmentCost> Discount(IEnumerable<ShipmentCost> pricedShipments)
        {
            var newEntries = pricedShipments.Select(MinimizeSmallPackagePrice);
            return _underlying.Discount(newEntries);
        }

        private ShipmentCost MinimizeSmallPackagePrice(ShipmentCost x)
        {
            if (x.Shipment.IsCorrupt) return x;

            if (x.Shipment.PackageSize == _discountedPackageSize)
            {
                var oldDiscount = x.Discount;
                var oldPrice = x.Price;
                var newPrice = oldPrice > _minCost ? _minCost : oldPrice;
                var newDiscount = oldPrice - newPrice + oldDiscount;
                return new ShipmentCost(x.Shipment, newPrice, newDiscount);
            }

            return x;

        }
    }
}