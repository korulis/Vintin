namespace Discounts.ApplicableDiscounts
{
    public class ShipmentWithFullDiscount : ShipmentWithApplicableDiscount
    {
        private readonly ShipmentCost _shipmentCost;

        public ShipmentWithFullDiscount(ShipmentCost shipmentCost)
        {
            _shipmentCost = shipmentCost;
        }

        public ShipmentCost Apply()
        {
            return new ShipmentCost(_shipmentCost.Shipment, 0, _shipmentCost.Discount + _shipmentCost.Price);
        }
    }
}