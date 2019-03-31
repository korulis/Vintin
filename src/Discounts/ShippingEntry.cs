using System;
using System.Collections.Generic;

namespace Discounts
{
    public class ShippingEntry
    {
        public IReadOnlyList<string> EntryElements { get; }
        public bool IsCorrupt { get; }
        public DateTime Date { get; set; }
        public string PackageSize { get; set; }
        public string ShippingProvider { get; set; }

        public ShippingEntry()
        {
            IsCorrupt = false;
        }

        private ShippingEntry(IReadOnlyList<string> entryElements)
        {
            EntryElements = entryElements;
            IsCorrupt = true;
        }

        public static ShippingEntry Corrupt(IReadOnlyList<string> lineElements)
        {
            return new ShippingEntry(lineElements);
        }
    }
}