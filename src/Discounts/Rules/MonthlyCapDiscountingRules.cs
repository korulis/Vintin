using Discounts.ApplicableDiscounts;
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

        public ShipmentWithApplicableDiscount AssignDiscount(IShipmentCost<IShipment> shipmentCost)
        {
            if (shipmentCost is IgnoredShipmentCost corruptShipmentCost)
            {
                return new DiscountForIgnoredShipment(corruptShipmentCost);
            }
            else if (shipmentCost is GoodShipmentCost goodShipmentCost)
            {
                var shipment = goodShipmentCost.Shipment;

                var incomingDiscount = goodShipmentCost.Discount;
                var month = Month(shipment);

                var oldTotalDiscount = GetCreateDiscount(month);

                if (ShouldBlockAnyFurtherDiscounting(oldTotalDiscount))
                {
                    var newnewTotalDiscount = _monthlyCap;
                    throw new Exception("ups");
                }

                ;

                if (_monthlyCap >= incomingDiscount + oldTotalDiscount)
                {
                    return new ShipmentWithNoAdditionalDiscount(goodShipmentCost);
                }
                else
                {
                    var newTargetDiscount = _monthlyCap - oldTotalDiscount;
                    return new ShipmentWithDiminishedDiscount(goodShipmentCost, newTargetDiscount);
                }
            }

            throw new NotImplementedException();
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

        public void Update(IShipmentCost<IShipment> shipmentCost)
        {
            if (shipmentCost is IgnoredShipmentCost)
            {
                return;
            }

            if (shipmentCost is GoodShipmentCost g)
            {

                var incomingDiscount = g.Discount;
                var month = Month(g.Shipment);
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