using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Discounts;

namespace ConsoleApp
{
    public class ShippingEntryMapper
    {
        private readonly string _separator;
        private static string _acceptableDateFormat;

        public ShippingEntryMapper(string separator, string acceptableDateFormat)
        {
            _separator = separator;
            _acceptableDateFormat = acceptableDateFormat;
        }

        public IEnumerable<ShippingEntry> ParseInput(IEnumerable<string> lines)
        {
            return lines.Select(ToShippingEntry);
        }

        public ShippingEntry ToShippingEntry(string line)
        {
            var lineElements = line.Split(_separator);

            var isDatePresent = DateTime.TryParseExact(
                lineElements[0],
                _acceptableDateFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var date);

            if (lineElements.Length != 3 || !isDatePresent)
            {
                return ShippingEntry.Corrupt(lineElements);
            }

            var result = new ShippingEntry
            {
                Date = date,
                PackageSize = lineElements[1],
                ShippingProvider = lineElements[2]
            };
            return result;
        }

        public IEnumerable<string> FormatOutput(IEnumerable<DiscountedShippingEntry> discountedShippingEntries)
        {
            return discountedShippingEntries.Select(FormatDiscounted);
        }

        private string FormatDiscounted(DiscountedShippingEntry discounted)
        {
            var shippingEntry = discounted.ShippingEntry;
            if (shippingEntry.IsCorrupt)
            {
                return string.Join(
                    _separator,
                    string.Join(_separator, shippingEntry.EntryElements),
                    "Ignored");
            }
            else
            {
                return string.Join(
                    _separator,
                    shippingEntry.Date.ToShortDateString(),
                    shippingEntry.PackageSize,
                    shippingEntry.ShippingProvider,
                    discounted.ShippingCost,
                    discounted.Discount);
            }
        }
    }
}