using System.Collections.Generic;
using System.Linq;

namespace Discounts.Discounters
{
    public class TenLimitDiscounter : IDiscounter
    {
        private readonly IDiscounter _underlying;
        private const int MaxMonthlyDiscount = 10;

        public TenLimitDiscounter(IDiscounter underlying)
        {
            _underlying = underlying;
        }

        public IEnumerable<ShippingCostEntry> Discount(IEnumerable<ShippingCostEntry> pricedShippingEntries)
        {
            var enumerated = pricedShippingEntries.ToList();
            var newEntries = enumerated.Select((x, i) => LimitDiscountsToTenPerMonth(x, i, enumerated));
            return _underlying.Discount(newEntries);
        }

        private static ShippingCostEntry LimitDiscountsToTenPerMonth(
            ShippingCostEntry entry,
            int index,
            IEnumerable<ShippingCostEntry> allEntries)
        {
            if (entry.ShippingEntry.IsCorrupt) return entry;
            var previousEntries = allEntries.Take(index);

            var year = entry.ShippingEntry.Date.Year;
            var month = entry.ShippingEntry.Date.Month;
            var discount = entry.Discount;
            var price = entry.Price;

            var totalDiscountThisMonth = previousEntries
                                        .Where(x => x.ShippingEntry.Date.Year == year
                                                    && x.ShippingEntry.Date.Month == month)
                                        .Select(x => x.Discount)
                                        .Sum();

            if (ShouldBlockAnyFurtherDiscounting(totalDiscountThisMonth))
            {
                return new ShippingCostEntry(entry.ShippingEntry, entry.Price + entry.Discount, 0.0m);
            };

            var maxAllowedDiscountForCurrentEntry = MaxMonthlyDiscount - totalDiscountThisMonth;
            if (maxAllowedDiscountForCurrentEntry >= discount)
            {
                return entry;
            }

            var newDiscount = maxAllowedDiscountForCurrentEntry;
            var newPrice = price + discount - newDiscount;
            return new ShippingCostEntry(entry.ShippingEntry, newPrice, newDiscount);
        }

        private static bool ShouldBlockAnyFurtherDiscounting(decimal totalDiscountThisMonth)
        {
            return !(MaxMonthlyDiscount >= totalDiscountThisMonth);
        }
    }
}