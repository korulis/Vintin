using System;
using System.Collections.Generic;

namespace Discounts.Filters
{
    public class OncePerMonthDiscountingRules : DiscountingRules
    {
        private readonly Dictionary<DateTime, int> _precedingEntries = new Dictionary<DateTime, int>();
        private const string SpecialProvider = "LP";
        private const string SpecialSize = "L";
        private const int LuckyOrderNumber = 3;

        public ShipmentWithApplicableDiscount AssignDiscount(ShipmentCost shipmentCost)
        {
            if (shipmentCost.Shipment.IsCorrupt)
            {
                return new ShipmentWithFullDiscount(shipmentCost);
            }

            var month = Month(shipmentCost.Shipment);

            if (_precedingEntries.TryGetValue(month, out var count) && count == LuckyOrderNumber - 1)
            {
                return new ShipmentWithFullDiscount(shipmentCost);
            }

            return new ShipmentWithNoDiscount(shipmentCost);
        }

        public void Update(ShipmentCost shipmentCost)
        {
            var shipment = shipmentCost.Shipment;
            if (shipment.IsCorrupt) return;
            if (shipment.PackageSize != SpecialSize) return; ;
            if (shipment.ShippingProvider != SpecialProvider) return;

            var month = Month(shipment);
            if (_precedingEntries.ContainsKey(month))
            {
                _precedingEntries[month]++;
            }
            else
            {
                _precedingEntries.Add(month, 1);
            }
        }

        private static DateTime Month(Shipment shipment)
        {
            return new DateTime(shipment.Date.Year, shipment.Date.Month, 1);
        }
    }
}