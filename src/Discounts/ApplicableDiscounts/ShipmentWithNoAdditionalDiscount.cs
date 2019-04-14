namespace Discounts.ApplicableDiscounts
{
    public class ShipmentWithNoAdditionalDiscount : ShipmentWithApplicableDiscount
    {
        private readonly GoodShipmentCost _shipmentCost;

        public ShipmentWithNoAdditionalDiscount(GoodShipmentCost shipmentCost)
        {
            _shipmentCost = shipmentCost;
        }

        public IShipmentCost<IShipment> Apply()
        {
                return new GoodShipmentCost(
                    _shipmentCost.Shipment,
                    _shipmentCost.Price,
                    _shipmentCost.Discount);
        }
    }
}