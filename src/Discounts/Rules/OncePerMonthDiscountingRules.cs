using System;
using System.Collections.Generic;
using Discounts.ApplicableDiscounts;

namespace Discounts.Rules
{
    public class OncePerMonthDiscountingRules : DiscountingRules
    {
        private readonly string _specialProvider;
        private readonly string _specialSize;
        private readonly int _luckOrderNumber;

        private readonly Dictionary<DateTime, int> _context = new Dictionary<DateTime, int>();

        public OncePerMonthDiscountingRules(string specialProvider, string specialSize, int luckOrderNumber)
        {
            _specialProvider = specialProvider;
            _specialSize = specialSize;
            _luckOrderNumber = luckOrderNumber;
        }

        public ShipmentWithApplicableDiscount AssignDiscount(ShipmentCost shipmentCost)
        {
            if (shipmentCost.Shipment.IsCorrupt)
            {
                return new DiscountForCorruptShipment(shipmentCost);
            }

            var shipment = shipmentCost.Shipment;
            var month = Month(shipmentCost.Shipment);

            if (_context.TryGetValue(month, out var count) 
                && shipment.PackageSize == _specialSize
                && shipment.ShippingProvider == _specialProvider
                && count == _luckOrderNumber - 1)
            {
                return new ShipmentWithFullDiscount(shipmentCost);
            }

            return new ShipmentWithNoAdditionalDiscount(shipmentCost);
        }

        public void Update(ShipmentCost shipmentCost)
        {
            var shipment = shipmentCost.Shipment;
            if (shipment.IsCorrupt) return;
            if (shipment.PackageSize != _specialSize) return; ;
            if (shipment.ShippingProvider != _specialProvider) return;


            var month = Month(shipment);
            if (_context.ContainsKey(month))
            {
                _context[month]++;
            }
            else
            {
                _context.Add(month, 1);
            }
        }

        private static DateTime Month(Shipment shipment)
        {
            return new DateTime(shipment.Date.Year, shipment.Date.Month, 1);
        }
    }
}