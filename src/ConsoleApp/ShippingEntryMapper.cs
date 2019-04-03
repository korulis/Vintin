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
        private static string _dateFormat;
        private static readonly string[] AcceptableSizes = {"S", "M", "L"};
        private static readonly string[] AcceptableProviders = {"MR", "LP"};

        public ShippingEntryMapper(string separator, string dateFormat)
        {
            _separator = separator;
            _dateFormat = dateFormat;
        }

        public IEnumerable<ShippingEntry> ParseInput(IEnumerable<string> lines)
        {
            return lines.Select(ToShippingEntry);
        }

        private ShippingEntry ToShippingEntry(string line)
        {
            var lineElements = line.Split(_separator);

            if (lineElements.Length != 3)
            {
                return ShippingEntry.Corrupt(line);
            }


            var isDatePresent = DateTime.TryParseExact(
                lineElements[0],
                _dateFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var date);

            var size = lineElements[1];
            var provider = lineElements[2];

            if (!(isDatePresent 
                  && AcceptableProviders.Contains(provider) 
                  && AcceptableSizes.Contains(size)))
            {
                return ShippingEntry.Corrupt(line);
            }

            var result = new ShippingEntry
            {
                Date = date,
                PackageSize = size,
                ShippingProvider = provider
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
                    string.Join(_separator, shippingEntry.RawEntry),
                    "Ignored");
            }
            else
            {
                return string.Join(
                    _separator,
                    shippingEntry.Date.ToString(_dateFormat),
                    shippingEntry.PackageSize,
                    shippingEntry.ShippingProvider,
                    discounted.ShippingCost,
                    discounted.Discount);
            }
        }
    }
}