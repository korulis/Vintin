namespace Discounts
{
    public class ShippingCostEntry
    {
        public ShippingEntry ShippingEntry { get; }
        public decimal Price { get; }
        public decimal Discount { get; }

        public ShippingCostEntry(ShippingEntry shippingEntry, decimal price, decimal discount)
        {
            ShippingEntry = shippingEntry;
            Price = price;
            Discount = discount;
        }
    }
}