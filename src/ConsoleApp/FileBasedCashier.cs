using System;
using System.Collections.Generic;
using System.IO;
using Discounts;

namespace ConsoleApp
{
    public class FileBasedCashier
    {
        private readonly ShippingEntryMapper _shippingEntryMapper;
        private readonly ShippingPriceCalculator _shippingPriceCalculator;
        private readonly Action<IEnumerable<string>> _outputMethod;

        public FileBasedCashier(
            ShippingEntryMapper shippingEntryMapper,
            ShippingPriceCalculator shippingPriceCalculator,
            Action<IEnumerable<string>> outputMethod)
        {
            _shippingEntryMapper = shippingEntryMapper;
            _shippingPriceCalculator = shippingPriceCalculator;
            _outputMethod = outputMethod;
        }

        public void Process(string inputFilePath)
        {
            var shippingEntryLines = File.ReadLines(inputFilePath);

            var shippingEntries = _shippingEntryMapper.ParseInput(shippingEntryLines);

            var discountedShippingEntries = _shippingPriceCalculator.CalculatePrice(shippingEntries);

            var shippingCostsLines = _shippingEntryMapper.FormatOutput(discountedShippingEntries);

            _outputMethod(shippingCostsLines);
        }
    }
}