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

        public override bool Equals(object obj)
        {
            if (obj is ShipmentCost other)
            {
                return Shipment.Equals(other.Shipment)
                       && Price == other.Price
                       && Discount == other.Discount;
            }

            return base.Equals(obj);
        }
    }
}