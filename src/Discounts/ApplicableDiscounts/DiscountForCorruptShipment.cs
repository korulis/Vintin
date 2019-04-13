namespace Discounts.ApplicableDiscounts
{
    public class DiscountForCorruptShipment : ShipmentWithApplicableDiscount
    {
        private readonly IShipmentCost<IShipment> _goodShipmentCost;

        public DiscountForCorruptShipment(IShipmentCost<IShipment> goodShipmentCost)
        {
            _goodShipmentCost = goodShipmentCost;
        }

        public IShipmentCost<IShipment> Apply()
        {
            return _goodShipmentCost;
        }
    }
}