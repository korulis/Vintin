using System.Collections.Generic;
using System.IO;
using System.Linq;
using Discounts;
using Xunit;

namespace ConsoleApp.Tests
{
    public class AcceptanceTests
    {
        private const string InputFilePath = "input.txt";

        [Fact]
        public void FreeShippingTestCase()
        {
            //Arrange
            var expectedDiscountedLines = File.ReadLines("free-shipping-output.txt").ToArray();
            const string outputFilePath = "actual-free-shipping-output.txt";
            var fileBasedShippingPriceCalculator = BuildFileBasedShippingPriceCalculator(
                new FullDiscounts(),
                outputFilePath);

            //Act
            fileBasedShippingPriceCalculator.Process(InputFilePath);

            //Assert
            var actualDiscountedEntries = File.ReadLines(outputFilePath).ToArray();
            for (var i = 0; i < expectedDiscountedLines.Length; i++)
            {
                Assert.Equal(expectedDiscountedLines[i], actualDiscountedEntries[i]);
            }
            Assert.Equal(expectedDiscountedLines, actualDiscountedEntries);
        }

        [Fact]
        public void GreedyShippingTestCase()
        {
            //Arrange
            var expectedDiscountedLines = File.ReadLines("greedy-shipping-output.txt").ToArray();
            const string outputFilePath = "actual-greedy-shipping-output.txt";
            var fileBasedShippingPriceCalculator = BuildFileBasedShippingPriceCalculator(
                new NoDiscounts(),
                outputFilePath);

            //Act
            fileBasedShippingPriceCalculator.Process(InputFilePath);

            //Assert
            var actualDiscountedEntries = File.ReadLines(outputFilePath).ToArray();
            for (var i = 0; i < expectedDiscountedLines.Length; i++)
            {
                Assert.Equal(expectedDiscountedLines[i], actualDiscountedEntries[i]);
            }
            Assert.Equal(expectedDiscountedLines, actualDiscountedEntries);
        }
        private static FileBasedShippingPriceCalculator BuildFileBasedShippingPriceCalculator(
            IDiscounter discounter,
            string outputFilePath)
        {
            const string acceptableDateFormat = "yyyy-MM-dd";
            const string separator = " ";

            var shippingEntryParser = new ShippingEntryMapper(separator, acceptableDateFormat);
            var shippingCostCalculator = new ShippingPriceCalculator(discounter);
            void OutputMethod(IEnumerable<string> x) => File.WriteAllLines(outputFilePath, x);

            var fileBasedShippingPriceCalculator = new FileBasedShippingPriceCalculator(
                shippingEntryParser,
                shippingCostCalculator,
                OutputMethod);
            return fileBasedShippingPriceCalculator;
        }
    }
}
