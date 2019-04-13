namespace Discounts.ApplicableDiscounts
{
    public class ShipmentWithDiminishedDiscount : ShipmentWithApplicableDiscount
    {
        private readonly GoodShipmentCost _goodShipmentCost;
        private readonly decimal _targetDiscount;

        public ShipmentWithDiminishedDiscount(GoodShipmentCost goodShipmentCost, decimal targetDiscount)
        {
            _goodShipmentCost = goodShipmentCost;
            _targetDiscount = targetDiscount;
        }

        public IShipmentCost<IShipment> Apply()
        {
            return new GoodShipmentCost(_goodShipmentCost.Shipment, _goodShipmentCost.Price + _goodShipmentCost.Discount - _targetDiscount, _targetDiscount);
        }
    }
}