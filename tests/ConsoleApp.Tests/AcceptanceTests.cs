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
        private const string OutputFilePath = "output.txt";

        public static TheoryData<string, IDiscounter> CalculatorData =>
            new TheoryData<string, IDiscounter>
            {
                {"free-shipping-output.txt" , new CompleteDiscounts()},
                {"greedy-shipping-output.txt" , new NoDiscounts()},
                {"discounted-shipping-output.txt" , new SmallPackageLowestPriceDiscounter()},
            };

        [Theory]
        [MemberData(nameof(CalculatorData))]
        public void ShippingCostCalculationTests(string expectedOutputFile, IDiscounter discounter)
        {
            //Arrange
            var expectedDiscountedLines = File.ReadLines(expectedOutputFile).ToArray();
            var fileBasedShippingCostCalculator = BuildFileBasedShippingCostCalculator(discounter);

            //Act
            fileBasedShippingCostCalculator.Process(InputFilePath);

            //Assert
            var actualDiscountedEntries = File.ReadLines(OutputFilePath).ToArray();
            for (var i = 0; i < expectedDiscountedLines.Length; i++)
            {
                Assert.Equal(expectedDiscountedLines[i], actualDiscountedEntries[i]);
            }
            Assert.Equal(expectedDiscountedLines, actualDiscountedEntries);
        }

        private static FileBasedShippingCostCalculator BuildFileBasedShippingCostCalculator(IDiscounter discounter)
        {
            const string acceptableDateFormat = "yyyy-MM-dd";
            const string separator = " ";

            var shippingEntryParser = new ShippingEntryMapper(separator, acceptableDateFormat);
            var shippingCostCalculator = new ShippingCostCalculator(discounter);
            void OutputMethod(IEnumerable<string> x) => File.WriteAllLines(OutputFilePath, x);

            var fileBasedShippingPriceCalculator = new FileBasedShippingCostCalculator(
                shippingEntryParser,
                shippingCostCalculator,
                OutputMethod);
            return fileBasedShippingPriceCalculator;
        }
    }
}
