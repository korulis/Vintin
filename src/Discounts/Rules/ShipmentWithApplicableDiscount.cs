namespace Discounts.Rules
{
    public interface ShipmentWithApplicableDiscount
    {
        ShipmentCost Apply();
    }
}