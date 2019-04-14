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
            return x.MinimizeCostForSize(_minCost, _discountedPackageSize);
        }

    }
}