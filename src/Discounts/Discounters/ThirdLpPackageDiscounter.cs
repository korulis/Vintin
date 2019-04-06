
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Discounts.Discounters
{
    public class ThirdLpPackageDiscounter : IDiscounter
    {
        private static string _specialProvider = "LP";
        private static string _specialSize = "L";
        private static int _luckyOrderNumber = 3;

        private static decimal packageCost = Constants.CostReference[(_specialSize, _specialProvider)];

        public IEnumerable<ShippingCostEntry> Discount(IEnumerable<ShippingCostEntry> pricedShippingEntries)
        {

            var enumerated = pricedShippingEntries.ToList();
            var shippingCostEntries = enumerated.Select((x, i) => DiscountLpLargeThirdPerMonth(x, i, enumerated));
            return shippingCostEntries;
        }

        private static ShippingCostEntry DiscountLpLargeThirdPerMonth(
            ShippingCostEntry entry,
            int currentIndex,
            IEnumerable<ShippingCostEntry> list)
        {
            if (entry.ShippingEntry.IsCorrupt) return entry;

            var year = entry.ShippingEntry.Date.Year;
            var month = entry.ShippingEntry.Date.Month;

            var shippingCostEntries = list.Take(currentIndex);
            var isCurrentTheThird = shippingCostEntries
                .Count(x =>
                {
                    var b = x.ShippingEntry.Date.Year == year
                            && x.ShippingEntry.Date.Month == month
                            && x.ShippingEntry.ShippingProvider == _specialProvider
                            && x.ShippingEntry.PackageSize == _specialSize;
                    if (b)
                    {
                        return b;
                    }
                    return b;
                }) == _luckyOrderNumber - 1;


            if (isCurrentTheThird)
                return new ShippingCostEntry(entry.ShippingEntry, 0.0m, packageCost);
            else
                return entry;
        }
    }
}