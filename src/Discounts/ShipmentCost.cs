namespace Discounts
{
    public class ShipmentCost
    {
        public Shipment Shipment { get; }
        public decimal Price { get; }
        public decimal Discount { get; }

        public ShipmentCost(Shipment shipment, decimal price, decimal discount)
        {
            Shipment = shipment;
            Price = price;
            Discount = discount;
        }
    }
}