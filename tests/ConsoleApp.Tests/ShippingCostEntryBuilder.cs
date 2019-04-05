using System;
using System.Collections.Generic;
using Discounts;

namespace ConsoleApp.Tests
{
    public class ShippingCostEntryBuilder
    {
        private readonly Dictionary<(string, string), decimal> _costReference = Constants.CostReference;
        private DateTime _dateTime;
        private string _packageSize;
        private string _shippingProvider;
        private ShippingEntry _shippingEntry;
        private decimal _price;
        private decimal _discount;

        public ShippingCostEntry Build()
        {

            _shippingEntry = new ShippingEntry { Date = _dateTime, PackageSize = _packageSize, ShippingProvider = _shippingProvider };
            var result = new ShippingCostEntry(_shippingEntry, _price, _discount);
            return result;
        }

        public ShippingCostEntryBuilder()
        {
            _dateTime = new DateTime(2000, 1, 1);
            _packageSize = "L";
            _shippingProvider = "LP";
            _price = _costReference[(_packageSize, _shippingProvider)];
            _discount = 0.0m;
        }

        public ShippingCostEntryBuilder AddDays(int days)
        {
            _dateTime = _dateTime.AddDays(days);
            return this;
        }

        public ShippingCostEntryBuilder OnDay(int day)
        {
            _dateTime = new DateTime(_dateTime.Year, _dateTime.Month, day);
            return this;
        }

        public ShippingCostEntryBuilder WithProvider(string provider)
        {
            _shippingProvider = provider;
            return this;
        }

        public ShippingCostEntryBuilder WithSize(string size)
        {
            _packageSize = size;
            return this;
        }

        public ShippingCostEntryBuilder WithPricing(decimal price, decimal discount)
        {
            _price = price;
            _discount = discount;
            return this;
        }
    }
}