namespace Discounts.ApplicableDiscounts
{
    public class ShipmentWithDiminishedDiscount : ShipmentWithApplicableDiscount
    {
        private readonly ShipmentCost _shipmentCost;
        private readonly decimal _targetDiscount;

        public ShipmentWithDiminishedDiscount(ShipmentCost shipmentCost, decimal targetDiscount)
        {
            _shipmentCost = shipmentCost;
            _targetDiscount = targetDiscount;
        }

        public ShipmentCost Apply()
        {
            return new ShipmentCost(_shipmentCost.Shipment, _shipmentCost.Price + _shipmentCost.Discount - _targetDiscount, _targetDiscount);
        }
    }
}