using System;
using System.Collections.Generic;
using Discounts;

namespace ConsoleApp.Tests
{
    public class ShipmentCostBuilder
    {
        private readonly Dictionary<(string, string), decimal> _costReference = Defaults.CostReference;
        private DateTime _dateTime;
        private string _packageSize;
        private string _shippingProvider;
        private Shipment _shipment;
        private decimal _price;
        private decimal _discount;

        public ShipmentCost Build()
        {

            _shipment = new Shipment { Date = _dateTime, PackageSize = _packageSize, ShippingProvider = _shippingProvider };
            var result = new ShipmentCost(_shipment, _price, _discount);
            return result;
        }

        public ShipmentCostBuilder()
        {
            _dateTime = new DateTime(2000, 6, 1);
            _packageSize = "L";
            _shippingProvider = "LP";
            _price = _costReference[(_packageSize, _shippingProvider)];
            _discount = 0.0m;
        }

        public ShipmentCostBuilder OnMonth(int month)
        {
            _dateTime = new DateTime(_dateTime.Year, month, _dateTime.Day);
            return this;
        }

        public ShipmentCostBuilder OnDay(int day)
        {
            _dateTime = new DateTime(_dateTime.Year, _dateTime.Month, day);
            return this;
        }

        public ShipmentCostBuilder WithProvider(string provider)
        {
            _shippingProvider = provider;
            return this;
        }

        public ShipmentCostBuilder WithSize(string size)
        {
            _packageSize = size;
            return this;
        }

        public ShipmentCostBuilder WithPricing(decimal price, decimal discount)
        {
            _price = price;
            _discount = discount;
            return this;
        }
    }
}