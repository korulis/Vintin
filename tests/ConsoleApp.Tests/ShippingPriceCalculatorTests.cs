using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Discounts;
using Xunit;

namespace ConsoleApp.Tests
{
    public class ShippingPriceCalculatorTests
    {

        [Theory]
        [InlineData("S", "LP", "1.50")]
        [InlineData("M", "LP", "4.90")]
        [InlineData("L", "LP", "6.90")]
        [InlineData("S", "MR", "2.00")]
        [InlineData("M", "MR", "3.00")]
        [InlineData("L", "MR", "4.00")]
        public void CostsWithoutDiscount(string packageSize, string shippingProvider, string expectedCostString)
        {
            //Arrange
            var expectedCost = Convert.ToDecimal(expectedCostString, CultureInfo.InvariantCulture);
            var sut = new ShippingPriceCalculator();
            var shippingEntries = new List<ShippingEntry>()
            {
                new ShippingEntry()
                {
                    Date = DateTime.Today,
                    PackageSize = packageSize,
                    ShippingProvider = shippingProvider
                }
            };

            //Act
            var actual = sut.CalculatePrice(shippingEntries).ToList();

            //Assert
            Assert.Equal(expectedCost, actual[0].ShippingCost);
        }
    }
}