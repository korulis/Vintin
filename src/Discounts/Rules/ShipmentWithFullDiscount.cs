namespace Discounts.Rules
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
            if (_shipmentCost.Shipment.IsCorrupt)
            {
                return _shipmentCost;
            }

            return new ShipmentCost(_shipmentCost.Shipment, 0, _shipmentCost.Discount + _shipmentCost.Price);
        }
    }
}