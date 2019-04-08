namespace Discounts.Rules
{
    public class DiscountForCorruptShipment : ShipmentWithApplicableDiscount
    {
        private readonly ShipmentCost _shipmentCost;

        public DiscountForCorruptShipment(ShipmentCost shipmentCost)
        {
            _shipmentCost = shipmentCost;
        }

        public ShipmentCost Apply()
        {
            return _shipmentCost;
        }
    }
}