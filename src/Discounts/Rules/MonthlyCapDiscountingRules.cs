using System;
using System.Collections.Generic;

namespace Discounts.Rules
{
    public class MonthlyCapDiscountingRules : DiscountingRules
    {
        private readonly int _monthlyCap;

        private readonly Dictionary<DateTime, decimal> _context = new Dictionary<DateTime, decimal>();

        public MonthlyCapDiscountingRules(int monthlyCap)
        {
            _monthlyCap = monthlyCap;
        }

        public ShipmentWithApplicableDiscount AssignDiscount(ShipmentCost shipmentCost)
        {
            var shipment = shipmentCost.Shipment;
            if (shipment.IsCorrupt) return new DiscountForCorruptShipment(shipmentCost);

            var incomingDiscount = shipmentCost.Discount;
            var month = Month(shipment);

            var oldTotalDiscount = GetCreateDiscount(month);

            if (ShouldBlockAnyFurtherDiscounting(oldTotalDiscount))
            {
                var newnewTotalDiscount = _monthlyCap;
                throw new Exception("ups");
            };

            if (_monthlyCap >= incomingDiscount + oldTotalDiscount)
            {
                return new ShipmentWithNoAdditionalDiscount(shipmentCost);
            }
            else
            {
                var newTargetDiscount = _monthlyCap - oldTotalDiscount;
                return new ShipmentWithDiminishedDiscount(shipmentCost, newTargetDiscount);
            }
        }

        private decimal GetCreateDiscount(DateTime month)
        {
            decimal oldTotalDiscount;
            if (_context.ContainsKey(month))
            {
                oldTotalDiscount = _context[month];
            }
            else
            {
                _context.Add(month, 0);
                oldTotalDiscount = 0;
            }

            return oldTotalDiscount;
        }

        public void Update(ShipmentCost shipmentCost)
        {
            var shipment = shipmentCost.Shipment;
            if (shipment.IsCorrupt) return;

            var incomingDiscount = shipmentCost.Discount;
            var month = Month(shipment);
            decimal oldTotalDiscount;
            if (_context.ContainsKey(month))
            {
                oldTotalDiscount = _context[month];
            }
            else
            {
                _context.Add(month, 0);
                oldTotalDiscount = 0;
            }

            if (ShouldBlockAnyFurtherDiscounting(oldTotalDiscount))
            {
                var newnewTotalDiscount = _monthlyCap;
                throw new Exception("ups");
            };

            if (_monthlyCap >= incomingDiscount + oldTotalDiscount)
            {
                _context[month] = oldTotalDiscount + incomingDiscount;
            }
            else
            {
                _context[month] = _monthlyCap;
            }
        }

        private bool ShouldBlockAnyFurtherDiscounting(decimal totalExistingDiscount)
        {
            return _monthlyCap < totalExistingDiscount;
        }

        private static DateTime Month(Shipment shipment)
        {
            return new DateTime(shipment.Date.Year, shipment.Date.Month, 1);
        }
    }
}