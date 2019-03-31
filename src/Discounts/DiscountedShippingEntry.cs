namespace Discounts
{
    public class DiscountedShippingEntry
    {
        public ShippingEntry ShippingEntry { get; }
        public string ShippingCost { get; }
        public string Discount { get; }

        public DiscountedShippingEntry(ShippingEntry shippingEntry, string shippingCost, string discount)
        {
            ShippingEntry = shippingEntry;
            ShippingCost = shippingCost;
            Discount = discount;
        }
    }
}