using System;
using System.Collections.Generic;
using System.IO;
using Discounts;

namespace ConsoleApp
{
    public class FileBasedShippingCostCalculator
    {
        private readonly ShippingEntryMapper _shippingEntryMapper;
        private readonly ShippingCostCalculator _shippingCostCalculator;
        private readonly Action<IEnumerable<string>> _outputMethod;

        public FileBasedShippingCostCalculator(
            ShippingEntryMapper shippingEntryMapper,
            ShippingCostCalculator shippingCostCalculator,
            Action<IEnumerable<string>> outputMethod)
        {
            _shippingEntryMapper = shippingEntryMapper;
            _shippingCostCalculator = shippingCostCalculator;
            _outputMethod = outputMethod;
        }

        public void Process(string inputFilePath)
        {
            var shippingEntryLines = File.ReadLines(inputFilePath);

            var shippingEntries = _shippingEntryMapper.ParseInput(shippingEntryLines);

            var discountedShippingEntries = _shippingCostCalculator.CalculatePrice(shippingEntries);

            var shippingCostsLines = _shippingEntryMapper.FormatOutput(discountedShippingEntries);

            _outputMethod(shippingCostsLines);
        }
    }
}