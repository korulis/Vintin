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