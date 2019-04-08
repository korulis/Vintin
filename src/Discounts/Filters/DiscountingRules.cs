﻿namespace Discounts.Filters
{
    /// <summary>
    /// Assigns a discount to be applied based on its internal rules.
    /// </summary>
    public interface DiscountingRules
    {
        ShipmentWithApplicableDiscount AssignDiscount(ShipmentCost shipmentCost);
        void Update(ShipmentCost shipmentCost);
    }
}