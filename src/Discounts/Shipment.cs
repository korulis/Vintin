using System;
using System.Collections.Generic;

namespace Discounts
{
    public class Shipment : IShipment
    {
        private readonly string _separator;
        private readonly string[] _dateFormats;
        public DateTime Date { get; set; }
        public string PackageSize { get; set; }
        public string ShippingProvider { get; set; }

        public Shipment(DateTime date, string package, string shippingProvider, string separator, string[] dateFormats)
        {
            _separator = separator;
            _dateFormats = dateFormats;
            Date = date;
            PackageSize = package;
            ShippingProvider = shippingProvider;
        }

        public string Format()
        {
            var result = string.Join(
                _separator,
                Date.ToString(_dateFormats[0]),
                PackageSize,
                ShippingProvider);
            return result;
        }

        public decimal GetCost(Dictionary<(string, string), decimal> costReference)
        {
            return costReference[(PackageSize, ShippingProvider)];
        }

    }

    public interface IShipment
    {
        string Format();
        decimal GetCost(Dictionary<(string, string), decimal> costReference);
    }

    public class CorruptShipment : IShipment
    {
        private readonly string _separator;
        private readonly string _entry;

        public CorruptShipment(string entry, string separator)
        {
            _separator = separator;
            _entry = entry;
        }

        public string Format()
        {
            return string.Join(
                _separator,
                string.Join(_separator, _entry),
                "Ignored");
        }

        public decimal GetCost(Dictionary<(string, string), decimal> costReference)
        {
            return 0;
        }

    }

}