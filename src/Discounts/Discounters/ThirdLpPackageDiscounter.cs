
using System.Collections.Generic;
using System.Linq;
using Discounts.Filters;

namespace Discounts.Discounters
{
    public class ThirdLpPackageDiscounter : Discounter
    {
        private const string SpecialProvider = "LP";
        private const string SpecialSize = "L";
        private const int LuckyOrderNumber = 3;
        private readonly Discounter _underlying;
        private readonly DiscountingRules _discountingRules;

        public ThirdLpPackageDiscounter(Discounter underlying, DiscountingRules discountingRules)
        {
            _underlying = underlying;
            _discountingRules = discountingRules;
        }

        //TODO: inject discountingRules factory in order to make it safe to call this function more the once per instance.
        public IEnumerable<ShipmentCost> Discount(IEnumerable<ShipmentCost> pricedShipments)
        {
            var asdf = pricedShipments.Select(x => _discountingRules.AssignDiscount(x)).ToList();


            var enumerated = pricedShipments.ToList();
            var shipmentCosts = enumerated.Select((x, i) => DiscountLpLargeThirdPerMonth(x, i, enumerated));
            return _underlying.Discount(shipmentCosts);
        }

        private ShipmentCost DiscountLpLargeThirdPerMonth(
            ShipmentCost entry,
            int index,
            IEnumerable<ShipmentCost> allEntries)
        {
            if (entry.Shipment.IsCorrupt) return entry;

            var previousEntries = allEntries.Take(index);
            var year = entry.Shipment.Date.Year;
            var month = entry.Shipment.Date.Month;

            var wouldCurrentBeTheThird = previousEntries
                .Count(x => x.Shipment.Date.Year == year
                            && x.Shipment.Date.Month == month
                            && x.Shipment.ShippingProvider == SpecialProvider
                            && x.Shipment.PackageSize == SpecialSize) == LuckyOrderNumber - 1;

            if (wouldCurrentBeTheThird
                && entry.Shipment.PackageSize == SpecialSize
                && entry.Shipment.ShippingProvider == SpecialProvider)
            {
                return FreeShipment(entry);
            }
            return entry;
        }

        private static ShipmentCost FreeShipment(ShipmentCost entry)
        {
            return new ShipmentCost(entry.Shipment, 0.0m, entry.Discount + entry.Price);
        }
    }
}