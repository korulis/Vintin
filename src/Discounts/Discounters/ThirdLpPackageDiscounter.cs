
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Discounts.Discounters
{
    public class ThirdLpPackageDiscounter : IDiscounter
    {
        private const string SpecialProvider = "LP";
        private const string SpecialSize = "L";
        private const int LuckyOrderNumber = 3;

        private static readonly decimal PackageCost = Constants.CostReference[(SpecialSize, SpecialProvider)];

        public IEnumerable<ShippingCostEntry> Discount(IEnumerable<ShippingCostEntry> pricedShippingEntries)
        {
            var enumerated = pricedShippingEntries.ToList();
            var shippingCostEntries = enumerated.Select((x, i) => DiscountLpLargeThirdPerMonth(x, enumerated.Take(i)));
            return shippingCostEntries;
        }

        private static ShippingCostEntry DiscountLpLargeThirdPerMonth(
            ShippingCostEntry entry,
            IEnumerable<ShippingCostEntry> previousEntries)
        {
            if (entry.ShippingEntry.IsCorrupt) return entry;

            var year = entry.ShippingEntry.Date.Year;
            var month = entry.ShippingEntry.Date.Month;

            var isCurrentTheThird = previousEntries
                .Count(x => x.ShippingEntry.Date.Year == year
                            && x.ShippingEntry.Date.Month == month
                            && x.ShippingEntry.ShippingProvider == SpecialProvider
                            && x.ShippingEntry.PackageSize == SpecialSize) == LuckyOrderNumber - 1;

            return isCurrentTheThird ? new ShippingCostEntry(entry.ShippingEntry, 0.0m, PackageCost) : entry;
        }
    }
}