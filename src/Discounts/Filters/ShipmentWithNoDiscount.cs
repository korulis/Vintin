namespace Discounts.Filters
{
    public class ShipmentWithNoDiscount : ShipmentWithApplicableDiscount
    {
        private readonly ShipmentCost _shipmentCost;

        public ShipmentWithNoDiscount(ShipmentCost shipmentCost)
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