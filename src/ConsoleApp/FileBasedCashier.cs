using System;
using System.Collections.Generic;
using System.IO;
using Discounts;

namespace ConsoleApp
{
    public class FileBasedCashier
    {
        private readonly ShippingEntryMapper _shippingEntryMapper;
        private readonly ShippingCostsCalculator _shippingCostsCalculator;
        private readonly Action<IEnumerable<string>> _outputMethod;

        public FileBasedCashier(
            ShippingEntryMapper shippingEntryMapper,
            ShippingCostsCalculator shippingCostsCalculator,
            Action<IEnumerable<string>> outputMethod)
        {
            _shippingEntryMapper = shippingEntryMapper;
            _shippingCostsCalculator = shippingCostsCalculator;
            _outputMethod = outputMethod;
        }

        public void Process(string inputFilePath)
        {
            var shippingEntryLines = File.ReadLines(inputFilePath);

            var shippingEntries = _shippingEntryMapper.ParseInput(shippingEntryLines);

            var discountedShippingEntries = _shippingCostsCalculator.CalculateCost(shippingEntries);

            var shippingCostsLines = _shippingEntryMapper.FormatOutput(discountedShippingEntries);

            _outputMethod(shippingCostsLines);
        }
    }
}