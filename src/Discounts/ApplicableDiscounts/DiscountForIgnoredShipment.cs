namespace Discounts.ApplicableDiscounts
{
    public class DiscountForIgnoredShipment : ShipmentWithApplicableDiscount
    {
        private readonly IgnoredShipmentCost _shipmentCost;

        public DiscountForIgnoredShipment(IgnoredShipmentCost shipmentCost)
        {
            _shipmentCost = shipmentCost;
        }

        public IShipmentCost<IShipment> Apply()
        {
            return _shipmentCost;
        }
    }
}