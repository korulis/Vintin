using System;

namespace Discounts
{
    public class ShippingEntry
    {
        public string RawEntry { get; }
        public bool IsCorrupt { get; }
        public DateTime Date { get; set; }
        public string PackageSize { get; set; }
        public string ShippingProvider { get; set; }

        public ShippingEntry()
        {
            IsCorrupt = false;
        }

        private ShippingEntry(string rawEntry)
        {
            RawEntry = rawEntry;
            IsCorrupt = true;
        }

        public static ShippingEntry Corrupt(string lineElements)
        {
            var shippingEntry = new ShippingEntry(lineElements);
            return shippingEntry;
        }
    }
}