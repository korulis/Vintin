using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Discounts;

namespace ConsoleApp
{
    public class ShipmentMapper
    {
        private readonly string _separator;
        private static string[] _dateFormats;
        private readonly string[] _acceptableProviders;
        private readonly string[] _acceptableSizes;

        public ShipmentMapper(string separator, string[] dateFormats, string[] acceptableProviders, string[] acceptableSizes)
        {

            _separator = separator;
            _dateFormats = dateFormats;
            _acceptableProviders = acceptableProviders;
            _acceptableSizes = acceptableSizes;
        }

        public IEnumerable<IShipment> ParseInput(IEnumerable<string> lines)
        {
            return lines.Select(ToShipment);
        }

        private IShipment ToShipment(string line)
        {
            var lineElements = line.Split(_separator);

            if (lineElements.Length != 3)
            {
                return new CorruptShipment(line, _separator);
            }


            var isDatePresent = TryParseDate(lineElements, out var date);

            var size = lineElements[1];
            var provider = lineElements[2];

            if (!(isDatePresent
                  && _acceptableProviders.Contains(provider)
                  && _acceptableSizes.Contains(size)))
            {
                return new CorruptShipment(line, _separator);
            }

            var result = new Shipment(date,size,provider,_separator,_dateFormats)
            {
                Date = date,
                PackageSize = size,
                ShippingProvider = provider
            };
            return result;
        }

        private static bool TryParseDate(string[] lineElements, out DateTime date)
        {
            var tryParseExact = DateTime.TryParseExact(
                lineElements[0],
                _dateFormats,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out date);
            return tryParseExact;
        }

        public IEnumerable<string> FormatOutput(IEnumerable<ShipmentCost> discountedShipments)
        {
            return discountedShipments.Select(FormatDiscounted);
        }

        private static string FormatDiscounted(ShipmentCost discounted)
        {
            var shipment = discounted.Shipment;
            return shipment.Format();
        }

    }
}