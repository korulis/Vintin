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

        public override bool Equals(object obj)
        {
            if (obj is ShippingEntry other)
            {
                var eq = IsCorrupt == other.IsCorrupt
                              && RawEntry == other.RawEntry
                              && Date == other.Date
                              && PackageSize == other.PackageSize
                              && ShippingProvider == other.ShippingProvider;
                if (eq)
                {
                    return eq;
                }
                return eq;
            }

            return base.Equals(obj);
        }

        //public override string ToString()
        //{
        //    return IsCorrupt.ToString()
        //           + " " + RawEntry.ToString()
        //           + " " + Date.ToString()
        //           + " " + PackageSize.ToString()
        //           + " " + ShippingProvider.ToString();
        //}

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