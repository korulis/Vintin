using System.Collections.Generic;
using System.Linq;

namespace Discounts.Discounters
{
    public class SmallPackageLowestPriceDiscounter : IDiscounter
    {
        private readonly IDiscounter _underlying;
        private readonly decimal _minCost;
        private readonly string _discountedPackageSize;

        public SmallPackageLowestPriceDiscounter(IDiscounter underlying,Dictionary<(string, string), decimal> sizeAndProviderToCost)
        {
            _underlying = underlying;
            _discountedPackageSize = "S";
            _minCost = sizeAndProviderToCost.Where(x => x.Key.Item1 == _discountedPackageSize).Select(x => x.Value).Min();
        }

        public IEnumerable<ShippingCostEntry> Discount(IEnumerable<ShippingCostEntry> pricedShippingEntries)
        {
            var newEntries = pricedShippingEntries.Select(MinimizeSmallPackagePrice);
            return _underlying.Discount(newEntries);
        }

        private ShippingCostEntry MinimizeSmallPackagePrice(ShippingCostEntry x)
        {
            if (x.ShippingEntry.IsCorrupt) return x;

            if (x.ShippingEntry.PackageSize == _discountedPackageSize)
            {
                var oldDiscount = x.Discount;
                var oldPrice = x.Price;
                var newPrice = oldPrice > _minCost ? _minCost : oldPrice;
                var newDiscount = oldPrice - newPrice + oldDiscount;
                return new ShippingCostEntry(x.ShippingEntry, newPrice, newDiscount);
            }

            return x;

        }
    }
}