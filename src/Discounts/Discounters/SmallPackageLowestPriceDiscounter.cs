using System.Collections.Generic;
using System.Linq;

namespace Discounts.Discounters
{
    public class SmallPackageLowestPriceDiscounter : IDiscounter
    {
        private readonly decimal _minCost;

        public SmallPackageLowestPriceDiscounter(Dictionary<(string, string), decimal> sizeAndProviderToCost)
        {
            _minCost = sizeAndProviderToCost.Where(x => x.Key.Item1 == "S").Select(x => x.Value).Min();
        }

        public IEnumerable<ShippingCostEntry> Discount(IEnumerable<ShippingCostEntry> pricedShippingEntries)
        {
            return pricedShippingEntries.Select(MinimizeSmallPackagePrice);
        }

        private ShippingCostEntry MinimizeSmallPackagePrice(ShippingCostEntry x)
        {
            if (x.ShippingEntry.IsCorrupt) return x;

            if (x.ShippingEntry.PackageSize == "S")
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