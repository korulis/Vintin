namespace Discounts
{
    public class ProcessedShippingEntry
    {
        public ShippingEntry ShippingEntry { get; }
        public decimal ShippingCost { get; }
        public decimal Discount { get; }

        public ProcessedShippingEntry(ShippingEntry shippingEntry, decimal shippingCost, decimal discount)
        {
            ShippingEntry = shippingEntry;
            ShippingCost = shippingCost;
            Discount = discount;
        }
    }
}