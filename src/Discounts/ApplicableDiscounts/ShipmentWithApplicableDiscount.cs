namespace Discounts.ApplicableDiscounts
{
    public interface ShipmentWithApplicableDiscount
    {
        IShipmentCost<IShipment> Apply();
    }
}