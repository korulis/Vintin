using System.Collections.Generic;
using System.Linq;

namespace Discounts.Discounters
{
    public class SmallPackageLowestPriceDiscounter : IDiscounter
    {
        private readonly Dictionary<(string, string), decimal> _costReference =
            new Dictionary<(string, string), decimal>
            {
                { ("S","LP"), 1.50m},
                { ("M","LP"), 4.90m},
                { ("L","LP"), 6.90m},
                { ("S","MR"), 2.00m},
                { ("M","MR"), 3.00m},
                { ("L","MR"), 4.00m}
            };

        private decimal _minCost;

        public SmallPackageLowestPriceDiscounter()
        {
            _minCost = _costReference.Where(x => x.Key.Item1 == "S").Select(x => x.Value).Min();
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