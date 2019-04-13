using Discounts.ApplicableDiscounts;
using System;
using System.Collections.Generic;

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

        public ShipmentWithApplicableDiscount AssignDiscount(IShipmentCost<IShipment> shipmentCost)
        {

            if (shipmentCost is CorruptShipmentCost)
            {
                return new DiscountForCorruptShipment(shipmentCost);
            }
            else if (shipmentCost is GoodShipmentCost goodShipmentCost)
            {
                var shipment = goodShipmentCost.Shipment;
                var month = Month(goodShipmentCost.Shipment);

                if (_context.TryGetValue(month, out var count)
                    && shipment.PackageSize == _specialSize
                    && shipment.ShippingProvider == _specialProvider
                    && count == _luckOrderNumber - 1)
                {
                    return new ShipmentWithFullDiscount(goodShipmentCost);
                }

                return new ShipmentWithNoAdditionalDiscount(goodShipmentCost);
            }
            throw new NotImplementedException();
        }

        public void Update(IShipmentCost<IShipment> shipmentCost)
        {
            if (shipmentCost is CorruptShipmentCost)
            {
                return;
            }

            if (shipmentCost is GoodShipmentCost goodShipmentCost)
            {
                var shipment = goodShipmentCost.Shipment;
                if (shipment.PackageSize != _specialSize) return;
                ;
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
        }

        private static DateTime Month(Shipment shipment)
        {
            return new DateTime(shipment.Date.Year, shipment.Date.Month, 1);
        }
    }
}