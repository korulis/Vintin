using System.Collections.Generic;
using System.Diagnostics;
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
            var newEntries = enumerated.Select((x, i) => LimitDiscountsToTenPerMonth(x, enumerated.Take(i)));
            return _underlying.Discount(newEntries);
        }

        private static ShippingCostEntry LimitDiscountsToTenPerMonth(ShippingCostEntry entry, IEnumerable<ShippingCostEntry> previousEntries)
        {
            if (entry.ShippingEntry.IsCorrupt) return entry;

            var year = entry.ShippingEntry.Date.Year;
            var month = entry.ShippingEntry.Date.Month;
            var discount = entry.Discount;
            var price = entry.Price;

            var totalDiscountThisMonth = previousEntries
                                        .Where(x => x.ShippingEntry.Date.Year == year
                                                    && x.ShippingEntry.Date.Month == month)
                                        .Select(x => x.Discount)
                                        .Sum();

            Debug.Assert(MaxMonthlyDiscount >= totalDiscountThisMonth);

            var maxAllowedDiscountForCurrentEntry = MaxMonthlyDiscount - totalDiscountThisMonth;
            if (maxAllowedDiscountForCurrentEntry >= discount)
            {
                return entry;
            }

            var newDiscount = maxAllowedDiscountForCurrentEntry;
            var newPrice = price + discount - newDiscount;
            return new ShippingCostEntry(entry.ShippingEntry, newPrice, newDiscount);
        }
    }
}