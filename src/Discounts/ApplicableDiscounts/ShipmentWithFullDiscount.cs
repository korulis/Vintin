namespace Discounts.ApplicableDiscounts
{
    public class ShipmentWithFullDiscount : ShipmentWithApplicableDiscount
    {
        private readonly GoodShipmentCost _goodShipmentCost;

        public ShipmentWithFullDiscount(GoodShipmentCost goodShipmentCost)
        {
            _goodShipmentCost = goodShipmentCost;
        }

        public IShipmentCost<IShipment> Apply()
        {
            var goodShipmentCost = new GoodShipmentCost(
                _goodShipmentCost.Shipment,
                0,
                _goodShipmentCost.Discount + _goodShipmentCost.Price);
            return goodShipmentCost;
        }
    }
}