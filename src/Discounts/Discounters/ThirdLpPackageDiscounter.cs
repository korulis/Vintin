
using System;
using System.Collections.Generic;
using System.Linq;
using Discounts.Rules;

namespace Discounts.Discounters
{
    public class ThirdLpPackageDiscounter : Discounter
    {
        private readonly Discounter _underlying;
        private readonly Func<DiscountingRules> _discountingRulesSpawner;

        public ThirdLpPackageDiscounter(Discounter underlying, Func<DiscountingRules> discountingRulesSpawner)
        {
            _underlying = underlying;
            _discountingRulesSpawner = discountingRulesSpawner;
        }

        public IEnumerable<ShipmentCost> Discount(IEnumerable<ShipmentCost> pricedShipments)
        {
            var rules = _discountingRulesSpawner();
            var shipmentCosts = pricedShipments
                .Select(x => rules.AssignDiscount(x))
                .Select(x => x.Apply())
                .Select(x=>
                {
                    rules.Update(x);
                    return x;
                })
                .ToList();


            return _underlying.Discount(shipmentCosts);
        }
    }
}