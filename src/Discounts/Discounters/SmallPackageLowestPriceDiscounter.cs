using System.Collections.Generic;
using System.Linq;

namespace Discounts.Discounters
{
    public class SmallPackageLowestPriceDiscounter : Discounter
    {
        private readonly Discounter _underlying;
        private readonly decimal _minCost;
        private readonly string _discountedPackageSize;

        public SmallPackageLowestPriceDiscounter(
            Discounter underlying,
            Dictionary<(string, string), decimal> sizeAndProviderToCost,
            string targetSize)
        {
            _underlying = underlying;
            _discountedPackageSize = targetSize;
            _minCost = sizeAndProviderToCost.Where(x => x.Key.Item1 == _discountedPackageSize).Select(x => x.Value).Min();
        }

        public IEnumerable<IShipmentCost<IShipment>> Discount(IEnumerable<IShipmentCost<IShipment>> pricedShipments)
        {
            var newEntries = pricedShipments.Select(MinimizeSmallPackagePrice);
            return _underlying.Discount(newEntries);
        }

        private IShipmentCost<IShipment> MinimizeSmallPackagePrice(IShipmentCost<IShipment> x)
        {
            if (x is IgnoredShipmentCost)
            {
                return x;
            }

            if (x is GoodShipmentCost g)
            {
                if (g.Shipment.PackageSize == _discountedPackageSize)
                {
                    var oldDiscount = g.Discount;
                    var oldPrice = g.Price;
                    var newPrice = oldPrice > _minCost ? _minCost : oldPrice;
                    var newDiscount = oldPrice - newPrice + oldDiscount;
                    return new GoodShipmentCost(g.Shipment, newPrice, newDiscount);
                }

                return x;
            }

            return x;
        }

    }
}