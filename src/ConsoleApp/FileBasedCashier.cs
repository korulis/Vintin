using System.IO;
using Discounts;

namespace ConsoleApp.Tests
{
    public class FileBasedCashier
    {
        private readonly ShippingEntryMapper _shippingEntryMapper;
        private readonly ShippingCostsCalculator _shippingCostsCalculator;

        public FileBasedCashier(
            ShippingEntryMapper shippingEntryMapper,
            ShippingCostsCalculator shippingCostsCalculator)
        {
            _shippingEntryMapper = shippingEntryMapper;
            _shippingCostsCalculator = shippingCostsCalculator;
        }

        public void Process(string inputFilePath, string outputFilePath)
        {
            var shippingEntryLines = File.ReadLines(inputFilePath);

            var shippingEntries = _shippingEntryMapper.ParseInput(shippingEntryLines);

            var discountedShippingEntries = _shippingCostsCalculator.CalculateCost(shippingEntries);

            var shippingCostsLines = _shippingEntryMapper.FormatOutput(discountedShippingEntries);

            File.WriteAllLines(outputFilePath, shippingCostsLines);
        }
    }
}