namespace Discounts.Filters
{
    public interface ShipmentWithApplicableDiscount
    {
        ShipmentCost Apply();
    }
}