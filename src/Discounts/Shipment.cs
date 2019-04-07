using System;

namespace Discounts
{
    public class Shipment
    {
        public string RawEntry { get; }
        public bool IsCorrupt { get; }
        public DateTime Date { get; set; }
        public string PackageSize { get; set; }
        public string ShippingProvider { get; set; }

        public Shipment()
        {
            IsCorrupt = false;
        }

        public override bool Equals(object obj)
        {
            if (obj is Shipment other)
            {
                return IsCorrupt == other.IsCorrupt
                       && RawEntry == other.RawEntry
                       && Date == other.Date
                       && PackageSize == other.PackageSize
                       && ShippingProvider == other.ShippingProvider;
            }

            return base.Equals(obj);
        }

        private Shipment(string rawEntry, bool isCorrupt)
        {
            RawEntry = rawEntry;
            IsCorrupt = isCorrupt;
        }

        public static Shipment Corrupt(string lineElements)
        {
            return new Shipment(lineElements, true);
        }
    }
}