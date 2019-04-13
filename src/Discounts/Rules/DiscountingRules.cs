using Discounts.ApplicableDiscounts;

namespace Discounts.Rules
{
    /// <summary>
    /// Assigns a discount to be applied based on its internal rules.
    /// </summary>
    public interface DiscountingRules
    {
        ShipmentWithApplicableDiscount AssignDiscount(IShipmentCost<IShipment> shipmentCost);
        void Update(IShipmentCost<IShipment> shipmentCost);
    }
}