using System.IO;
using System.Linq;
using Discounts;
using Xunit;

namespace ConsoleApp.Tests
{
    public class AcceptanceTests
    {
        [Fact]
        public void FreeShippingTestCase()
        {
            //Arrange
            var expectedDiscountedLines = File.ReadLines("free-shipping-output.txt").ToArray();

            const string inputFilePath = "input.txt";
            const string acceptableDateFormat = "yyyy-MM-dd";
            const string separator = " ";
            const string outputFilePath = "actual-free-shipping-output.txt";

            var shippingEntryParser = new ShippingEntryMapper(separator, acceptableDateFormat);
            var shippingCostCalculator = new ShippingPriceCalculator();

            var fileProcessor = new FileBasedCashier(
                shippingEntryParser,
                shippingCostCalculator, 
                (x => { File.WriteAllLines("output.txt", x); }));

            //Act
            fileProcessor.Process(inputFilePath);

            //Assert
            var actualDiscountedEntries = File.ReadLines(outputFilePath).ToArray();
            for (int i = 0; i < expectedDiscountedLines.Length; i++)
            {
                Assert.Equal(expectedDiscountedLines[i], actualDiscountedEntries[i]);
            }
            Assert.Equal(expectedDiscountedLines, actualDiscountedEntries);
        }
    }
}
