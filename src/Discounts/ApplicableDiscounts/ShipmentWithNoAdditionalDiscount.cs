using System;

namespace Discounts.ApplicableDiscounts
{
    public class ShipmentWithNoAdditionalDiscount : ShipmentWithApplicableDiscount
    {
        private readonly IShipmentCost<IShipment> _shipmentCost;

        public ShipmentWithNoAdditionalDiscount(IShipmentCost<IShipment> shipmentCost)
        {
            _shipmentCost = shipmentCost;
        }

        public IShipmentCost<IShipment> Apply()
        {
            if (_shipmentCost is CorruptShipmentCost)
            {
                return _shipmentCost;
            }
            else if (_shipmentCost is GoodShipmentCost goodShipmentCost)
            {
                return new GoodShipmentCost(
                    goodShipmentCost.Shipment,
                    goodShipmentCost.Price,
                    goodShipmentCost.Discount);
            }
            throw new NotImplementedException();
        }
    }
}