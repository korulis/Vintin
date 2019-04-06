
using System.Collections.Generic;
using System.Linq;

namespace Discounts.Discounters
{
    public class ThirdLpPackageDiscounter : IDiscounter
    {
        private readonly IDiscounter _underlying;
        private const string SpecialProvider = "LP";
        private const string SpecialSize = "L";
        private const int LuckyOrderNumber = 3;

        private static readonly decimal PackageCost = Constants.CostReference[(SpecialSize, SpecialProvider)];

        public ThirdLpPackageDiscounter(IDiscounter underlying)
        {
            _underlying = underlying;
        }

        public IEnumerable<ShippingCostEntry> Discount(IEnumerable<ShippingCostEntry> pricedShippingEntries)
        {
            var enumerated = pricedShippingEntries.ToList();
            var shippingCostEntries = enumerated.Select((x, i) => DiscountLpLargeThirdPerMonth(x, i, enumerated));
            return _underlying.Discount(shippingCostEntries);
        }

        private static ShippingCostEntry DiscountLpLargeThirdPerMonth(
            ShippingCostEntry entry,
            int index,
            IEnumerable<ShippingCostEntry> allEntries)
        {
            if (entry.ShippingEntry.IsCorrupt) return entry;

            var previousEntries = allEntries.Take(index);
            var year = entry.ShippingEntry.Date.Year;
            var month = entry.ShippingEntry.Date.Month;

            var wouldCurrentBeTheThird = previousEntries
                .Count(x => x.ShippingEntry.Date.Year == year
                            && x.ShippingEntry.Date.Month == month
                            && x.ShippingEntry.ShippingProvider == SpecialProvider
                            && x.ShippingEntry.PackageSize == SpecialSize) == LuckyOrderNumber - 1;

            if (wouldCurrentBeTheThird
                && entry.ShippingEntry.PackageSize == SpecialSize
                && entry.ShippingEntry.ShippingProvider == SpecialProvider)
            {
                return new ShippingCostEntry(entry.ShippingEntry, 0.0m, PackageCost);
            }
            return entry;
        }
    }
}