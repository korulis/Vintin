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

        public IEnumerable<ShipmentCost> Discount(IEnumerable<ShipmentCost> pricedShipment)
        {
            var enumerated = pricedShipment.ToList();
            var newEntries = enumerated.Select((x, i) => LimitDiscountsToTenPerMonth(x, i, enumerated));
            return _underlying.Discount(newEntries);
        }

        private static ShipmentCost LimitDiscountsToTenPerMonth(
            ShipmentCost entry,
            int index,
            IEnumerable<ShipmentCost> allEntries)
        {
            if (entry.Shipment.IsCorrupt) return entry;
            var previousEntries = allEntries.Take(index);

            var year = entry.Shipment.Date.Year;
            var month = entry.Shipment.Date.Month;
            var discount = entry.Discount;
            var price = entry.Price;

            var totalDiscountThisMonth = previousEntries
                                        .Where(x => x.Shipment.Date.Year == year
                                                    && x.Shipment.Date.Month == month)
                                        .Select(x => x.Discount)
                                        .Sum();

            if (ShouldBlockAnyFurtherDiscounting(totalDiscountThisMonth))
            {
                return new ShipmentCost(entry.Shipment, entry.Price + entry.Discount, 0.0m);
            };

            var maxAllowedDiscountForCurrentEntry = MaxMonthlyDiscount - totalDiscountThisMonth;
            if (maxAllowedDiscountForCurrentEntry >= discount)
            {
                return entry;
            }

            var newDiscount = maxAllowedDiscountForCurrentEntry;
            var newPrice = price + discount - newDiscount;
            return new ShipmentCost(entry.Shipment, newPrice, newDiscount);
        }

        private static bool ShouldBlockAnyFurtherDiscounting(decimal totalDiscountThisMonth)
        {
            return !(MaxMonthlyDiscount >= totalDiscountThisMonth);
        }
    }
}