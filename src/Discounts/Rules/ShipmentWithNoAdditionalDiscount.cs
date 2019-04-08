namespace Discounts.Rules
{
    public class ShipmentWithNoAdditionalDiscount : ShipmentWithApplicableDiscount
    {
        private readonly ShipmentCost _shipmentCost;

        public ShipmentWithNoAdditionalDiscount(ShipmentCost shipmentCost)
        {
            _shipmentCost = shipmentCost;
        }

        public ShipmentCost Apply()
        {
            if (_shipmentCost.Shipment.IsCorrupt)
            {
                return _shipmentCost;
            }
            return new ShipmentCost(_shipmentCost.Shipment, _shipmentCost.Price, _shipmentCost.Discount);
        }
    }
}