
using System.Collections.Generic;
using System.Linq;

namespace Discounts.Discounters
{
    public class ThirdLpPackageDiscounter : IDiscounter
    {
        private const string SpecialProvider = "LP";
        private const string SpecialSize = "L";
        private const int LuckyOrderNumber = 3;
        private readonly IDiscounter _underlying;

        public ThirdLpPackageDiscounter(IDiscounter underlying)
        {
            _underlying = underlying;
        }

        public IEnumerable<ShipmentCost> Discount(IEnumerable<ShipmentCost> pricedShipment)
        {
            var enumerated = pricedShipment.ToList();
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