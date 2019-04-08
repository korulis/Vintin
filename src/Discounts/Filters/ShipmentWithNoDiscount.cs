namespace Discounts.Filters
{
    public class ShipmentWithNoDiscount : ShipmentWithApplicableDiscount
    {
        public ShipmentCost Apply()
        {
            throw new System.NotImplementedException();
        }
    }
}