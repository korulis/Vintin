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
        private static string _dateFormat;
        private static readonly string[] AcceptableSizes = { "S", "M", "L" };
        private static readonly string[] AcceptableProviders = { "MR", "LP" };

        public ShipmentMapper(string separator, string dateFormat)
        {
            _separator = separator;
            _dateFormat = dateFormat;
        }

        public IEnumerable<Shipment> ParseInput(IEnumerable<string> lines)
        {
            return lines.Select(ToShipment);
        }

        private Shipment ToShipment(string line)
        {
            var lineElements = line.Split(_separator);

            if (lineElements.Length != 3)
            {
                return Shipment.Corrupt(line);
            }


            var isDatePresent = TryParseDate(lineElements, out var date);

            var size = lineElements[1];
            var provider = lineElements[2];

            if (!(isDatePresent
                  && AcceptableProviders.Contains(provider)
                  && AcceptableSizes.Contains(size)))
            {
                return Shipment.Corrupt(line);
            }

            var result = new Shipment
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
                _dateFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out date);
            return tryParseExact;
        }

        public IEnumerable<string> FormatOutput(IEnumerable<ShipmentCost> discountedShipments)
        {
            return discountedShipments.Select(FormatDiscounted);
        }

        private string FormatDiscounted(ShipmentCost priced)
        {
            var shipment = priced.Shipment;
            if (shipment.IsCorrupt)
            {
                return string.Join(
                    _separator,
                    string.Join(_separator, shipment.RawEntry),
                    "Ignored");
            }
            else
            {
                var discount = priced.Discount;
                return string.Join(
                    _separator,
                    shipment.Date.ToString(_dateFormat),
                    shipment.PackageSize,
                    shipment.ShippingProvider,
                    priced.Price.ToString("F", CultureInfo.InvariantCulture),
                    discount == 0 ? "-" : discount.ToString("F", CultureInfo.InvariantCulture));
            }
        }
    }
}