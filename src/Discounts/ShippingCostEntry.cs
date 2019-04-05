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

        public override bool Equals(object obj)
        {
            if (obj is ShippingCostEntry other)
            {
                var eq = ShippingEntry.Equals(other.ShippingEntry)
                              && Price == other.Price
                              && Discount == other.Discount;
                if (eq)
                {
                    return true;
                }
                return eq;
            }

            return base.Equals(obj);
        }

        //public override string ToString()
        //{
        //    return ShippingEntry.ToString()
        //           + ", " + Price.ToString()
        //           + ", " + Discount.ToString();
        //}
    }
}